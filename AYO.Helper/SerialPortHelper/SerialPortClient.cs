using System;
using System.Collections.Generic;
using System.IO.Ports;
using AYO.Helper.LogHelper;
using static AYO.Helper.SerialPortModel;

namespace AYO.Helper.SerialPortHelper
{
    public class SerialPortClient
    {
        #region 变量属性

        public event Action<List<byte>> DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;
        private readonly SerialPort _serialPort = new SerialPort();
        private string _portName = "COM1";//串口号，默认COM1
        private BaudRates _baudRates = BaudRates.BR_9600;//波特率
        private DataBits _dataBits = DataBits.Eight;//数据位
        private StopBits _stopBits = StopBits.One;//停止位
        private Parity _parity = Parity.None;//校验位
        private Handshake _handshake = Handshake.None;

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public BaudRates BaudRate
        {
            get { return _baudRates; }
            set { _baudRates = value; }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public DataBits DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        /// <summary>
        /// 流控
        /// </summary>
        public Handshake Handshake
        {
            get { return _handshake; }
            set { _handshake = value; }
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
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.ErrorReceived += SerialPort_ErrorReceived;
        }
        
        /// <summary>
        /// 参数构造函数（使用枚举参数构造）
        /// </summary>
        /// <param name="name">串口号</param>
        /// <param name="baud">波特率</param>
        /// <param name="dBits">数据位</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="handshake">流控</param>
        public SerialPortClient(string name, BaudRates baud, DataBits dBits, Parity par, StopBits sBits, Handshake handshake)
        {
            this._portName = name;
            this._baudRates = baud;
            this._parity = par;
            this._dataBits = dBits;
            this._stopBits = sBits;
            this._handshake = handshake;
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
        /// <param name="handshake">流控</param>
        public SerialPortClient(string name, string baud, string dBits, string par, string sBits, string handshake)
        {
            this._portName = name;
            this._baudRates = (BaudRates)Enum.Parse(typeof(BaudRates), baud);
            this._parity = (Parity)Enum.Parse(typeof(Parity), par);
            this._dataBits = (DataBits)Enum.Parse(typeof(DataBits), dBits);
            this._stopBits = (StopBits)Enum.Parse(typeof(StopBits), sBits);
            this._handshake = (Handshake)Enum.Parse(typeof(Handshake), handshake);
            BoundEvents();
        }

        #endregion 构造函数

        #region 事件处理函数

        /// <summary>
        /// 数据仓库
        /// </summary>
        private List<byte> _dataPool = new List<byte>();//存放接收的所有字节

        /// <summary>
        /// 数据接收处理
        /// </summary>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort.IsOpen)     //判断是否打开串口
            {
                Byte[] receivedData = new Byte[_serialPort.BytesToRead];        //创建接收字节数组
                _serialPort.Read(receivedData, 0, receivedData.Length);         //读取数据

                //触发整条记录的处理
                if (DataReceived != null)
                {
                    _dataPool.AddRange(receivedData);
                    DataReceived(_dataPool);
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
        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
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
                return _serialPort.IsOpen;
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            if (_serialPort.IsOpen) _serialPort.Close();
            _serialPort.PortName = _portName;
            _serialPort.BaudRate = (int)_baudRates;
            _serialPort.DataBits = (int)_dataBits;
            _serialPort.Parity = _parity;
            _serialPort.StopBits = _stopBits;
            _serialPort.Handshake = _handshake;
            _serialPort.Open();
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void Close()
        {
            if (_serialPort.IsOpen) _serialPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
        }

        #endregion 串口关闭/打开

        #region 写入数据
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入端口的字节数组</param>
        /// <param name="offset">偏移值</param>
        /// <param name="count">总数</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (!(_serialPort.IsOpen)) _serialPort.Open();
            _serialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入端口的字节数组</param>
        public void Write(byte[] buffer)
        {
            if (!(_serialPort.IsOpen)) _serialPort.Open();
            _serialPort.Write(buffer, 0, buffer.Length);
        }

        #endregion 写入数据
    }
}