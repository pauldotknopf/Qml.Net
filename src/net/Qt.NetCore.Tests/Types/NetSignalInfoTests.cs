﻿using FluentAssertions;
using Qt.NetCore.Types;
using Xunit;

namespace Qt.NetCore.Tests.Types
{
    public class NetSignalInfoTests
    {
        [Fact]
        public void Can_create_signal_info()
        {
            using (var signal = new NetSignalInfo("testSignal"))
            {
                signal.Name.Should().Be("testSignal");
                signal.ParameterCount.Should().Be(0);
                signal.GetParameter(0).Should().Be(NetVariantType.Invalid);
                signal.AddParameter(NetVariantType.Double);
                signal.ParameterCount.Should().Be(1);
                signal.GetParameter(0).Should().Be(NetVariantType.Double);
                signal.GetParameter(1).Should().Be(NetVariantType.Invalid);
            }
        }
    }
}