using System;
using System.Collections;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Models
{
    public class LevelMap
    {
        private RoomType[] data;
        
        public Vector2Int  size {
            get;
            private set;
        }
        public Vector2Int offset {
            get;
            set;
        }

        public LevelMap(Vector2Int size) : this(size, Vector2Int.zero)
        {}
        public LevelMap(Vector2Int size, Vector2Int offset)
        {
            this.size = size;
            this.offset = offset;
            data = new RoomType[size.x * size.y];
        }

        public int GetIndex(Vector2Int pos)
        {
            return pos.x + (size.x * pos.y);
        }

        public Vector2Int[] GetOfType(RoomType type)
        {
            ArrayList l = new ArrayList();
            foreach (var singlePos in new RectInt(Vector2Int.zero, size).allPositionsWithin)
            {
                if (this[singlePos] == type) l.Add(singlePos);
            }

            return (Vector2Int[]) l.ToArray(typeof(Vector2Int));
        }

        public bool InBounds(Vector2Int pos)
        {
            return new RectInt(Vector2Int.zero, size).Contains(pos + offset);
        }

        public Vector2Int GetVector(int index)
        {
            return new Vector2Int(index / size.y, index % size.x);
        }

        public bool IsEmpty(Vector2Int pos)
        {
            return this[pos] == RoomType.None;
        }

        public bool AcceptRoomAt(Vector2Int pos)
        {
            return AcceptRoomAt(pos, 1);
        }
        public bool AcceptRoomAt(Vector2Int pos, int border)
        {
            if (!InBounds(pos))
            {
                return false;
            }
            
            var span = new RectInt(pos + new Vector2Int(-border, -border), new Vector2Int(1 + 2 * border, 1 + 2 * border));
            foreach (var singlePos in span.allPositionsWithin)
            {
                var room = this[singlePos];
                if (room == RoomType.None || room == RoomType.Default) continue;
                return false;
            }

            return true;
        }

        public RoomType this[int x, int y]
        {
            get
            {
                return this[new Vector2Int(x, y)];
            }
            set
            {
                this[new Vector2Int(x, y)] = value;
            }
        }

        public RoomType this[Vector2Int pos]
        {
            get
            {
                pos += offset;
                try
                {
                    return data[GetIndex(pos)];
                }
                catch
                {
                    return RoomType.None;
                }
            }
            set
            {
                pos += offset;
                data[GetIndex(pos)] = value;
            }
        }

        public override string ToString()
        {
            string result = "";
            int y = 0;
            foreach (var singlePos in new RectInt(Vector2Int.zero, size).allPositionsWithin)
            {
                if (singlePos.y != y)
                {
                    result += Console.Out.NewLine;
                    y = singlePos.y;
                }
                result += Enum.GetName(typeof(RoomType), this[singlePos]).Substring(0, 1);
            }

            return result;
        }
    }
}