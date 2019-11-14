#region Usings

using System;

#endregion

namespace OneHundredAndEighty_2._0
{
    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; }
        public string NickName { get; }

        public Player(string name, string nickName, int id = -1)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(nickName))
            {
                //todo: message
                throw new Exception("Name or NickName can't be empty");
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