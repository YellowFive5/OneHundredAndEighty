#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore
{
    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; }
        public string NickName { get; }

        public Player(string name, string nickName, int id = -1)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(nickName) ||
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(nickName))
            {
                //todo: message
                throw new Exception("Name or NickName can't be empty, null or whitespace");
            }

            Id = id;
            Name = name;
            NickName = nickName;
        }


        public void SetId(int id)
        {
            Id = id;
        }
    }
}