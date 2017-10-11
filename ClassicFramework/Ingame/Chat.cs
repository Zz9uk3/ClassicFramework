using System;
using System.Collections.Generic;
using System.Text;
using ClassicFramework.Constants;
using ClassicFramework.Mem;

namespace ClassicFramework.Ingame
{
    /// <summary>
    /// Message class to store new chat messages
    /// </summary>
    internal class Message
    {
        internal string Owner { get; private set; }
        internal string Text { get; private set; }
        internal Enums.ChatType Type { get; private set; }

        internal Message(String parOwner, String parText, Enums.ChatType parType)
        {
            Owner = parOwner;
            Text = parText;
            Type = parType;
        }

        public override string ToString()
        {
            return Owner + " " + Type + " \"" + Text + "\"";
        }
    }

    /// <summary>
    /// class to manage the chat
    /// </summary>
    internal static class Chat
    {
        // Chat base static
        private static readonly IntPtr Base = new IntPtr(0xB50580);
        // Pointer to the last read message
        private static IntPtr CurMessage = Base;
        // overflow back to 0 after this address
        private static IntPtr LastMessage = new IntPtr(0x00B6DD80);
        // offset to next message from current one
        private static int nextMessage = 0x800;
        // list storing all messages we gathered
        private static List<Message> Messages = new List<Message>();

        /// <summary>
        /// Fetch all new messages beginning at the current message
        /// </summary>
        internal static void FetchMessages()
        {
            while (true)
            {
                // read first byte of current message
                byte tmpByte = Memory.Reader.Read<byte>(CurMessage);
                // if byte is 0x00 stop since we already read that message
                if (tmpByte == 0x00) return;

                // Otherwise read the whole message and split it
                // into its different information
                string[] tmpMessage = Memory.Reader.ReadString(CurMessage, Encoding.ASCII)
                    .Split(',');
                for (int i = 0; i < tmpMessage.Length; i++)
                {
                    tmpMessage[i] = tmpMessage[i]
                        .Split(new string[] { ": [" }, StringSplitOptions.None)[1]
                        .TrimEnd(']');
                }
                int tmpType = Convert.ToInt32(tmpMessage[0]);
                if (Enum.IsDefined(typeof(Enums.ChatType), tmpType))
                {
                    // Do something with the new message here
                }

                // set the current message to 0x00 since we already read it
                Memory.Reader.Write<byte>(CurMessage, 0x00);

                // overflow?
                if (CurMessage == LastMessage)
                {
                    // yes? set current message to the base
                    CurMessage = Base;
                }
                else
                {
                    // no? increment to next message
                    CurMessage = IntPtr.Add(CurMessage, nextMessage);
                }
            }
        }

        internal static void PrintMessage(string parMessage)
        {
            Functions.DoString("DEFAULT_CHAT_FRAME:AddMessage("
                + parMessage + ");");
        }

    }
}
