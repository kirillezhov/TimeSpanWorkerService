using System;

namespace TimeStampWorkerService
{
    public interface ITimeStampProvider
    {
        /// <summary>
        /// Возвращает текущую отметку времени.
        /// </summary>
        /// <remarks>
        /// Реализация должна:
        ///  - быть потокобезопасной
        ///  - возвращать отметки времени округлённые до 1 милисекунды
        ///  - допускается возвращение отметок времени из будущего, если в текущую миллисекунду отметка времени уже выдана.
        /// </remarks>
        /// <returns>Текущая отметка времени.</returns>
        DateTime TimeStamp();
    }
}