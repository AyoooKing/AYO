using System.Text;
using AYO.Helper.LogHelper;
using AYO.Helper.SerialPortHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AYO.UnitTest.SerialPortClientTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void SerialPortClientTest()
        {
            SerialPortClient portClient = new SerialPortClient("COM6", "9600", "8", "None", "1", "None");
            portClient.DataReceived += (_dataPool =>
            {
                while (true)
                {
                    if (_dataPool.Count < 4) break;
                    if ((_dataPool[0] == 0xea) && (_dataPool[0] == 0xeb))
                    {
                        _dataPool.RemoveAt(0);
                        continue;
                    }

                    int index = -1;//正确作法是记住上次最末尾的索引，避免重复匹配
                    for (int i = 0; i < _dataPool.Count - 1; i++)
                    {
                        if (_dataPool[i] == 0xfa && _dataPool[i + 1] == 0xfb)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index != -1)
                    {
                        byte[] data = _dataPool.GetRange(0, index + 2).ToArray();
                        _dataPool.RemoveRange(0, index + 2);
                        string msg = Encoding.UTF8.GetString(data, 2, data.Length - 4);
                        Log.Debug(msg);
                    }
                    else break;
                }
            });
        }
    }
}