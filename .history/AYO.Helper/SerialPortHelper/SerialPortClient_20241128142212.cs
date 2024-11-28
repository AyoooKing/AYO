using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AYO.Helper.SerialPortModel;

namespace AYO.Helper
{
    public class SerialPortClient
    {
        #region 变量属性

        public event Action<List<byte>> DataReceived;

        public event SerialErrorReceivedEventHandler ErrorReceived;

        private SerialPort serialPort = new SerialPort();
        private string portName = "COM1";//串口号，默认COM1
        private BaudRates baudRate = BaudRates.BR_9600;//波特率
        private DataBits dataBits = DataBits.Eight;//数据位
        private StopBits stopBits = StopBits.One;//停止位
        private Parity parity = Parity.None;//校验位
        private Handshake handshake = Handshake.None;

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public BaudRates BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public DataBits DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        /// <summary>
        /// 流控
        /// </summary>
        public Handshake Handshake
        {
            get { return handshake; }
            set { handshake = value; }
        }

        #endregion 变量属性

        #region 构造函数

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SerialPortClient()
        {
            BoundEvents();
        }

        private void BoundEvents()
        {
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
        }


        public SerialPortClient(string portName)
        {
            
        }

        /// <summary>
        /// 参数构造函数（使用枚举参数构造）
        /// </summary>
        /// <param name="name">串口号</param>
        /// <param name="baud">波特率</param>
        /// <param name="dBits">数据位</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="hhake">流控</param>
        public SerialPortClient(string name, BaudRates baud, DataBits dBits, Parity par, StopBits sBits, Handshake hhake)
        {
            this.portName = name;
            this.baudRate = baud;
            this.parity = par;
            this.dataBits = dBits;
            this.stopBits = sBits;
            this.handshake = hhake;
            BoundEvents();
        }

        /// <summary>
        /// 参数构造函数（使用字符串参数构造）
        /// </summary>
        /// <param name="name">串口号</param>
        /// <param name="baud">波特率</param>
        /// <param name="dBits">数据位</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="hhake">流控</param>
        public SerialPortClient(string name, string baud, string dBits, string par, string sBits, string hhake)
        {
            this.portName = name;
            this.baudRate = (BaudRates)Enum.Parse(typeof(BaudRates), baud);
            this.parity = (Parity)Enum.Parse(typeof(Parity), par);
            this.dataBits = (DataBits)Enum.Parse(typeof(DataBits), dBits);
            this.stopBits = (StopBits)Enum.Parse(typeof(StopBits), sBits);
            this.handshake = (Handshake)Enum.Parse(typeof(Handshake), hhake);
            BoundEvents();
        }

        #endregion 构造函数

        #region 事件处理函数

        /// <summary>
        /// 数据仓库
        /// </summary>
        private List<byte> datapool = new List<byte>();//存放接收的所有字节

        /// <summary>
        /// 数据接收处理
        /// </summary>
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)     //判断是否打开串口
            {
                Byte[] receivedData = new Byte[serialPort.BytesToRead];        //创建接收字节数组
                serialPort.Read(receivedData, 0, receivedData.Length);         //读取数据

                //触发整条记录的处理
                if (DataReceived != null)
                {
                    datapool.AddRange(receivedData);
                    DataReceived(datapool);
                }
            }
            else
            {
                Log.Warn($"请打开串口端口");
            }
        }

        /// <summary>
        /// 错误处理函数
        /// </summary>
        private void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(sender, e);
            }
        }

        #endregion 事件处理函数

        #region 串口关闭/打开

        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            if (serialPort.IsOpen) serialPort.Close();

            serialPort.PortName = portName;
            serialPort.BaudRate = (int)baudRate;
            serialPort.Parity = parity;
            serialPort.DataBits = (int)dataBits;
            serialPort.StopBits = stopBits;

            serialPort.Open();
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void Close()
        {
            if (serialPort.IsOpen) serialPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
        }

        #endregion 串口关闭/打开

        #region 写入数据

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (!(serialPort.IsOpen)) serialPort.Open();
            serialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入端口的字节数组</param>
        public void Write(byte[] buffer)
        {
            if (!(serialPort.IsOpen)) serialPort.Open();
            serialPort.Write(buffer, 0, buffer.Length);
        }

        #endregion 写入数据
    }
}