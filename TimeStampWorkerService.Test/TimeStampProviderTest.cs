using System;
using System.Linq;
using System.Threading;

using FluentAssertions;

using NUnit.Framework;

namespace TimeStampWorkerService.Test
{
    public class TimeStampProviderTest
    {
        /// <summary>
        /// Тестовый объект.
        /// </summary>
        private ITimeStampProvider _timeStampProvider;

        /// <summary>
        /// Настройка теста.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _timeStampProvider = new TimeStampProvider();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void TimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .Select(_ => _timeStampProvider.TimeStamp()).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void ConcurrentTimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .AsParallel()
                .Select(_ => _timeStampProvider.TimeStamp()).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void TimeStampProviderShouldReturnRoundedValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 1000)
                .AsParallel()
                .Select(_ => _timeStampProvider.TimeStamp()).ToArray();

            // assert
            foreach (var timeStamp in timeStamps)
            {
                (timeStamp.Ticks % TimeSpan.TicksPerMillisecond).Should().Be(0);
            }
        }

        /// <summary>
        /// Тест проверяет отсутствие залипания отметок времени в "прошлом".
        /// </summary>
        [Test]
        public void TimeStampIntervalTest()
        {
            // act
            var ts1 = _timeStampProvider.TimeStamp();
            Thread.Sleep(100);
            var ts2 = _timeStampProvider.TimeStamp();

            // assert
            (ts2 - ts1).TotalMilliseconds.Should().BeGreaterThan(90);
        }
    }
}