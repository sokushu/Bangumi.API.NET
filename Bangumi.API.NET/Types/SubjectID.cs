//MIT License

//Copyright (c) 2025 Sokushu

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;

namespace Bangumi.API.NET.Types
{
    /// <summary>
    /// Bangumi条目ID
    /// </summary>
    public readonly struct SubjectID
    {
        public const int MinID = 1;
        private readonly int ID;
        private SubjectID(int id)
        {
            if (id < MinID)
                throw new ArgumentOutOfRangeException(nameof(id), "ID cannot be negative.");
            ID = id;
        }

        public static implicit operator int(SubjectID id) => id.ID;

        public static implicit operator SubjectID(int id) => new SubjectID(id);

        public static bool operator ==(SubjectID left, SubjectID right) => left.ID == right.ID;

        public static bool operator !=(SubjectID left, SubjectID right) => left.ID != right.ID;

        public override bool Equals(object? obj) => 
            obj is SubjectID id ? ID == id.ID : obj is int intId && ID == intId;

        public override int GetHashCode() => ID.GetHashCode();

        public readonly override string ToString() => ID.ToString();
    }
}
