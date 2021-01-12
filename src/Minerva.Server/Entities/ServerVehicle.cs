﻿using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Minerva.Server.Enums;
using System;

namespace Minerva.Server.Entities
{
    public class ServerVehicle
        : Vehicle
    {
        public ServerVehicle(IntPtr nativePointer, ushort id)
            : base(nativePointer, id)
        {
        }

        public ServerVehicle(uint model, Position position, Rotation rotation)
            : base(model, position, rotation)
        {
        }

        public VehicleIndicator ActiveIndicators
        {
            get
            {
                if (!GetStreamSyncedMetaData("indicators", out int result))
                {
                    return VehicleIndicator.None;
                }

                return (VehicleIndicator)result;
            }
            set
            {
                SetStreamSyncedMetaData("indicators", (int)value);
            }
        }

        public DataAccessLayer.Models.Vehicle DbEntity { get; set; }
    }
}