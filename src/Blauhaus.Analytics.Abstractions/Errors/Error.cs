using System;

namespace Blauhaus.Analytics.Abstractions.Errors
{
    public class Error : IEquatable<Error>
    {

        private Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public Error(string description)
        {
            Code = GetType().Name;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }

        public override string ToString()
        {
            return $"{Code} ::: {Description}";
        }

        public static Error Deserialize(string serializedError)
        {
            var deserialized = serializedError.Split(new []{" ::: "}, StringSplitOptions.None);
            if (deserialized.Length != 2)
            {
                throw new ArgumentException($"Input {serializedError} is not a valid serialized Error");
            }
            return new Error(deserialized[0], deserialized[1]);
        }

        public bool Equals(Error? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Code == other.Code && Description == other.Description;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Error) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Code.GetHashCode() * 397) ^ Description.GetHashCode();
            }
        }

        public static bool operator ==(Error? left, Error? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Error? left, Error? right)
        {
            return !Equals(left, right);
        }
    }


    public static class StringExtensions
    {
        public static Error ToError(this string serializedError)
        {
            return Error.Deserialize(serializedError);
        }
    }
}