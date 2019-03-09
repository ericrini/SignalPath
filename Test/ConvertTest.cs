using System;
using NUnit.Framework;

namespace SignalPath.Test
{
    public class ConvertTest
    {
        [Test]
        public void GivenConvert_WhenBaseToHex_ThenOutputIsCorrect()
        {
            Assert.AreEqual("", Convert.HexToBase64(""));
            Assert.AreEqual("RXZpZGludA==", Convert.HexToBase64("45766964696e74"));
            Assert.AreEqual("AA==", Convert.HexToBase64("00"));
            Assert.AreEqual("AAA=", Convert.HexToBase64("0000"));
            Assert.AreEqual("AAAA", Convert.HexToBase64("000000"));
            Assert.AreEqual("AAAAAA==", Convert.HexToBase64("00000000"));

            // Different tools disagree about what this should actualy be.
            // I'm going from this method... https://en.wikipedia.org/wiki/Base64.
            Assert.AreEqual("/w==", Convert.HexToBase64("ff"));
            Assert.AreEqual("/w==", Convert.HexToBase64("FF"));
        }

        [Test]
        public void GivenConvert_WhenBaseToHexInvalidInput_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Convert.HexToBase64(null);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Convert.HexToBase64("0");
            });
        }
    }
}
