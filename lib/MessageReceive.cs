﻿using Ritsukage_Message_Client.lib.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Ritsukage_Message_Client.lib
{
    class MessageReceive
    {
        private static long recordMessageId;

        private static readonly ThreadStart ThreadDefine = new ThreadStart(ThreadCallback);
        private static Thread MessageReceiveThread;

        private static bool Working;

        private static MessageReceive Receiver;

        private static bool MissConnect = false;

        private static void ThreadCallback()
        {
            while (!MainWindow.StopApplication)
            {
                Thread.Sleep(MainWindow.user.ReceiveMessageDelay);
                if (!CheckMessage())
                    MissConnect = true;
                else
                    MissConnect = false;
            }
        }

        private MessageReceive()
        {}

        public static MessageReceive GetInstance(long id = 0)
        {
            recordMessageId = id;
            if (Receiver == null)
            {
                Receiver = new MessageReceive();
            }
            return Receiver;
        }

        public void Start() {
            if (!Working)
            {
                Working = true;
                MessageReceiveThread = new Thread(ThreadDefine);
                MessageReceiveThread.Start();
            }
        }
        public void Stop() {
            if (Working)
            {
                MessageReceiveThread.Abort();
                MessageReceiveThread = null;
                Working = false;
            }
        }

        public static bool CheckMessage()
        {
            MySQLHelper sql = new MySQLHelper();
            try
            {
                sql.Connect();
                string search = "CALL ReceiveMessage(" + MainWindow.user.QQ + ", \"" + MainWindow.user.Code + "\", " + recordMessageId + ")";
                sql.DoSQLCommand(search);
                var reader = sql.GetLastDataReader();
                if (reader == null)
                    throw new Exception("更新消息列表失败");
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    recordMessageId = id;
                    DateTime date = reader.GetDateTime(1);
                    string title = reader.GetString(2);
                    string contant = reader.GetString(3);
                    string markdown = reader.GetString(4);
                    bool isLink = (reader.GetByte(5) == 1);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow.Form.NewMessage(title, contant, markdown, isLink);
                    });
                    
                }
                sql.Disconnect();
            }
            catch (Exception ex)
            {
                sql.Disconnect();
                if (!MissConnect)
                    MainWindow.Form.TipError("小律影", "获取消息时发生异常：" + ex.ToString());
                return false;
            }
            return true;
        }
        private void CheckMessage(object sender, EventArgs e) => CheckMessage();
    }
}
