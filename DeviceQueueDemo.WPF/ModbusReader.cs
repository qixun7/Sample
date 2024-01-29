using NModbus;
using NModbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceQueueDemo.WPF
{
    public class ModbusReader
    {
        private IModbusMaster _master;

        public ModbusReader(IModbusMaster modbusMaster)
        {
            _master = modbusMaster;
        }

            //读取
        public ushort[] ReadModbusData(int slaveId, int address, int numberOfPoints)
        {
            if (address >= 1 && address <= 9999)
            {
                // 线圈 - 读取线圈状态
                return _master.ReadCoils((byte)slaveId, (ushort)(address - 1), (ushort)numberOfPoints).Select(b => (ushort)(b ? 1 : 0)).ToArray();
            }
            if (address >= 10001 && address <= 19999)
            {
                // 离散输入 - 读取离散输入状态
                return _master.ReadInputs((byte)slaveId, (ushort)(address - 10001), (ushort)numberOfPoints).Select(b => (ushort)(b ? 1 : 0)).ToArray();
            }
            if (address >= 30001 && address <= 39999)
            {
                // 输入寄存器 - 读取输入寄存器
                return _master.ReadInputRegisters((byte)slaveId, (ushort)(address - 30001), (ushort)numberOfPoints);
            }
            if (address >= 40001 && address <= 49999)
            {
                // 保持寄存器 - 读取保持寄存器
                return _master.ReadHoldingRegisters((byte)slaveId, (ushort)(address - 40001), (ushort)numberOfPoints);
            }
            else
            {
                throw new ArgumentException("无效的地址范围");
            }
        }

        public async Task<ushort[]> ReadModbusDataAsync(int slaveId, int address, int numberOfPoints)
        {
            if (address >= 1 && address <= 9999)
            {
                // 异步读取线圈状态
                var coils = await _master.ReadCoilsAsync((byte)slaveId, (ushort)(address - 1), (ushort)numberOfPoints);
                return coils.Select(b => (ushort)(b ? 1 : 0)).ToArray();
            }
            else if (address >= 10001 && address <= 19999)
            {
                // 异步读取离散输入状态
                var inputs = await _master.ReadInputsAsync((byte)slaveId, (ushort)(address - 10001), (ushort)numberOfPoints);
                return inputs.Select(b => (ushort)(b ? 1 : 0)).ToArray();
            }
            else if (address >= 30001 && address <= 39999)
            {
                // 异步读取输入寄存器
                return await _master.ReadInputRegistersAsync((byte)slaveId, (ushort)(address - 30001), (ushort)numberOfPoints);
            }
            else if (address >= 40001 && address <= 49999)
            {
                // 异步读取保持寄存器
                return await _master.ReadHoldingRegistersAsync((byte)slaveId, (ushort)(address - 40001), (ushort)numberOfPoints);
            }
            else
            {
                throw new ArgumentException("无效的地址范围");
            }
        }
    }
}