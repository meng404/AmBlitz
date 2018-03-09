using System;
using System.Diagnostics;
using System.Threading;
namespace AmBlitzCore.ToolKit
{
    [DebuggerStepThrough]
    public static class Ensure
    {
        public static T IsBetween<T>(T value, T min, T max, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                var message = string.Format("Value is not between {1} and {2}: {0}.", value, min, max);
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }
        public static T IsEqualTo<T>(T value, T comparand, string paramName)
        {
            if (!value.Equals(comparand))
            {
                var message = string.Format("Value is not equal to {1}: {0}.", value, comparand);
                throw new ArgumentException(message, paramName);
            }
            return value;
        }

        public static T IsGreaterThanOrEqualTo<T>(T value, T comparand, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(comparand) < 0)
            {
                var message = string.Format("Value is not greater than or equal to {1}: {0}.", value, comparand);
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }

        public static int IsGreaterThanOrEqualToZero(int value, string paramName)
        {
            if (value < 0)
            {
                var message = $"Value is not greater than or equal to 0: {value}.";
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }

        public static long IsGreaterThanOrEqualToZero(long value, string paramName)
        {
            if (value < 0)
            {
                var message = $"Value is not greater than or equal to 0: {value}.";
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }


        public static int IsGreaterThanZero(int value, string paramName)
        {
            if (value > 0) return value;
            var message = $"Value is not greater than zero: {value}.";
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        public static long IsGreaterThanZero(long value, string paramName)
        {
            if (value <= 0)
            {
                var message = string.Format("Value is not greater than zero: {0}.", value);
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }

        public static TimeSpan IsGreaterThanZero(TimeSpan value, string paramName)
        {
            if (value <= TimeSpan.Zero)
            {
                var message = $"Value is not greater than zero: {value}.";
                throw new ArgumentOutOfRangeException(paramName, message);
            }
            return value;
        }






        public static T IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, "Value cannot be null.");
            }
            return value;
        }

        public static string IsNotNullOrEmpty(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("Value cannot be empty.", paramName);
            }
            return value;
        }

        public static T IsNull<T>(T value, string paramName) where T : class
        {
            if (value != null)
            {
                throw new ArgumentNullException(paramName, "Value must be null.");
            }
            return null;
        }

        public static int? IsNullOrGreaterThanOrEqualToZero(int? value, string paramName)
        {
            if (value != null)
            {
                IsGreaterThanOrEqualToZero(value.Value, paramName);
            }
            return value;
        }


        public static long? IsNullOrGreaterThanOrEqualToZero(long? value, string paramName)
        {
            if (value != null)
            {
                IsGreaterThanOrEqualToZero(value.Value, paramName);
            }
            return value;
        }

        public static int? IsNullOrGreaterThanZero(int? value, string paramName)
        {
            if (value != null)
            {
                IsGreaterThanZero(value.Value, paramName);
            }
            return value;
        }


        public static long? IsNullOrGreaterThanZero(long? value, string paramName)
        {
            if (value != null)
            {
                IsGreaterThanZero(value.Value, paramName);
            }
            return value;
        }

        public static TimeSpan? IsNullOrGreaterThanZero(TimeSpan? value, string paramName)
        {
            if (value != null)
            {
                IsGreaterThanZero(value.Value, paramName);
            }
            return value;
        }

        public static string IsNullOrNotEmpty(string value, string paramName)
        {
            if (value != null && value == "")
            {
                throw new ArgumentException("Value cannot be empty.", paramName);
            }
            return value;
        }

        public static TimeSpan? IsNullOrValidTimeout(TimeSpan? value, string paramName)
        {
            if (value != null)
            {
                IsValidTimeout(value.Value, paramName);
            }
            return value;
        }


        public static TimeSpan IsValidTimeout(TimeSpan value, string paramName)
        {
            if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
            {
                var message = $"Invalid timeout: {value}.";
                throw new ArgumentException(message, paramName);
            }
            return value;
        }

        public static void That(bool assertion, string message)
        {
            if (!assertion)
            {
                throw new ArgumentException(message);
            }
        }

        public static void That(bool assertion, string message, string paramName)
        {
            if (!assertion)
            {
                throw new ArgumentException(message, paramName);
            }
        }

        public static T That<T>(T value, Func<T, bool> assertion, string paramName, string message)
        {
            if (!assertion(value))
            {
                throw new ArgumentException(message, paramName);
            }

            return value;
        }
    }
}
