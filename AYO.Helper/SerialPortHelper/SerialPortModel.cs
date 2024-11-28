using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYO.Helper
{
    public class SerialPortModel
    {
        #region 波特率、数据位的枚举

        /// <summary>
        /// 串口数据位列表（5,6,7,8）
        /// </summary>
        public enum DataBits : int
        {
            Five = 5,
            Six = 6,
            Sevent = 7,
            Eight = 8
        }

        /// <summary>
        /// 串口波特率列表。
        /// 75,110,150,300,600,1200,2400,4800,9600,14400,19200,28800,38400,56000,57600,
        /// 115200,128000,230400,256000
        /// </summary>
        public enum BaudRates : int
        {
            BR_75 = 75,
            BR_110 = 110,
            BR_150 = 150,
            BR_300 = 300,
            BR_600 = 600,
            BR_1200 = 1200,
            BR_2400 = 2400,
            BR_4800 = 4800,
            BR_9600 = 9600,
            BR_14400 = 14400,
            BR_19200 = 19200,
            BR_28800 = 28800,
            BR_38400 = 38400,
            BR_56000 = 56000,
            BR_57600 = 57600,
            BR_115200 = 115200,
            BR_128000 = 128000,
            BR_230400 = 230400,
            BR_256000 = 256000
        }

        #endregion 波特率、数据位的枚举
    }
}