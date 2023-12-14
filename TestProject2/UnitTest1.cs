using System;
using FluentAssertions;
using WpfLibrary1;
using Xunit;
namespace UnitTest
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator = new();
        [Fact]
        public void Slozenie_two_plus_two()
        {
            _calculator.s = "2+2\0";
            _calculator.i = 0;
            double result = _calculator.ProcE();
            Assert.Equal(4, result);
        }
        [Fact]
        public void Slozenie_two_plus_two_plus_two()
        {
            _calculator.s = "2+2+2\0";
            _calculator.i = 0; double result = _calculator.ProcE();
            Assert.Equal(6, result);
        }
        [Fact]
        public void Rasnoct_two_mines_two()
        {
            _calculator.s = "2-2\0"; _calculator.i = 0;
            double result = _calculator.ProcE(); Assert.Equal(0, result);
        }
        [Fact]
        public void Delenie_two_and_two()
        {
            _calculator.s = "2/2\0";
            _calculator.i = 0; double result = _calculator.ProcE();
            Assert.Equal(1, result);
        }
        [Fact]
        public void Umnozhenie_two_na_two()
        {
            _calculator.s = "2*2\0";
            _calculator.i = 0; double result = _calculator.ProcE();
            Assert.Equal(4, result);
        }
        [Fact]
        public void ProcC_return516()
        {
            _calculator.s = "516\0"; _calculator.i = 0;
            var result = _calculator.ProcC(); Assert.Equal(516, result);
        }
        [Fact]
        public void ProcC_i_equal_5()
        {
            _calculator.s = "12345\0"; _calculator.i = 0;
            var result = _calculator.ProcC(); Assert.Equal(5, _calculator.i);
        }
        [Fact]
        public void ProcC_i_equal_4()
        {
            _calculator.s = "1234+5\0";
            _calculator.i = 0; var result = _calculator.ProcC();
            Assert.Equal(4, _calculator.i);
        }
        [Fact]
        public void Sloznoe_dva_plus_dva_umnozhit_na_dva()
        {
            _calculator.s = "2+2*2\0"; _calculator.i = 0;
            double result = _calculator.ProcE(); Assert.Equal(6, result);
        }
        [Fact]
        public void Skobochki_dva_plus_dva_umnozhit_na_dva()
        {
            _calculator.s = "(2+2)*2\0";
            _calculator.i = 0; double result = _calculator.ProcE();
            Assert.Equal(8, result);
        }
    }
}