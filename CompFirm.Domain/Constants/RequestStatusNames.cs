using System;
using System.Collections.Generic;

namespace CompFirm.Domain.Constants
{
    public class RequestStatusNames
    {
        public const string Draft = "Черновик";
        public const string Created = "Создан";
        public const string InProcessing = "В обработке";
        public const string ReadyToRelease = "Готов к выдаче";
        public const string Gived = "Выдан";
        public const string Canceled = "Отменен";
        public const string Rejected = "Аннулирован";

        public static string[] UnableToCancelStatus
        {
            get
            {
                return new string[]
                {
                    Gived,
                    Canceled,
                    Rejected
                };
            }
        }

        public static Dictionary<string, string[]> StatusRoadMap = new Dictionary<string, string[]>
        {
            { Draft, new string[] { Created } },
            { Created, new string[] { InProcessing, Canceled, Rejected } },
            { InProcessing, new string[] { ReadyToRelease, Canceled, Rejected } },
            { ReadyToRelease, new string[] { Gived, Canceled, Rejected } },
            { Gived, Array.Empty<string>() },
            { Canceled, Array.Empty<string>() },
            { Rejected, Array.Empty<string>() },
        };
    }
}
