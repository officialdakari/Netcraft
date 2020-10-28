using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using global::System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace AdminUtilsN
{
    public class Automod
    {
        public Hashtable warns = new Hashtable();
        public List<Mute> mutes = new List<Mute>();
        private System.Collections.Specialized.StringCollection mutelist = new System.Collections.Specialized.StringCollection();

        public Automod()
        {
            NCSApi.PlayerChatEvent += OnPlayerChat;
        }

        private const string AUTOMOD_SWEAR_REGEX_MATCH = @"\b((у|[нз]а|(хитро|не)?вз?[ыьъ]|с[ьъ]|(и|ра)[зс]ъ?|(о[тб]|под)[ьъ]?|(.\B)+?[оаеи])?-?([её]б(?!о[рй])|и[пб][ае][тц]).*?|(н[иеа]|[дп]о|ра[зс]|з?а|с(ме)?|о(т|дно)?)?-?ху([яйиеёю]|ли(?!ган)).*?|(в[зы]|(три|два|четыре)жды|(н|сук)а)?-?бл(я(?!(х|ш[кн]|мб)[ауеыио]).*?|[еэ][дт]ь?)|(ра[сз]|[зн]а|[со]|вы?|п(р[ои]|од)|и[зс]ъ?|[ао]т)?п[иеё]зд.*?|(за)?п[ие]д[аое]?р((ас)?(и(ли)?[нщктл]ь?)?|(о(ч[еи])?)?к|юг)[ауеы]?|манд([ауеы]|ой|[ао]вошь?(е?к[ауе])?|юк(ов|[ауи])?)|муд([аио].*?|е?н([ьюия]|ей))|([нз]а|по)х|мля([тд]ь)?|м[ао]л[ао]фь[яию])\b";

        public void OnPlayerChat(NCore.netcraft.server.api.events.PlayerChatEventArgs e)
        {
            if (!e.GetMessage().StartsWith("/"))
            {
                string m = e.GetMessage();
                NCore.NetworkPlayer p = e.GetPlayer();
                if (mutelist.Contains(p.Username))
                {
                    Mute mute = null;
                    foreach (var mute1 in mutes)
                    {
                        if (mute1.Player.UUID == p.UUID)
                        {
                            mute = mute1;
                        }
                    }

                    if (mute != null)
                    {
                        if (!mute.IsTimedOut())
                        {
                            e.SetCancelled(true);
                            p.SendMessage($"You have been silenced until {mute.Mutetime.ToString()}: {mute.MuteReason}");
                        }
                    }
                } else
                {
                    Mute mute = null;
                    foreach (var mute1 in mutes)
                    {
                        if (mute1.Player.UUID == p.UUID)
                        {
                            mute = mute1;
                        }
                    }
                    if (mute != null) mutes.Remove(mute);
                }

                if (Regex.Match(m, AUTOMOD_SWEAR_REGEX_MATCH, RegexOptions.IgnoreCase).Success)
                {
                    if (!warns.Keys.Cast<string>().ToArray().Contains(p.Username))
                    {
                        warns.Add(p.Username, 1);
                    }
                    else
                    {
                        warns.Add(p.Username, ((int) warns[p.Username]) + 1);
                    }

                    Plugin.GetInstance().AdminMessage($"[AU] {p.Username} got warned for [Badwords]. He has {warns[p.Username].ToString()} warns.");
                    var w = warns[p.Username];
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(w, 3, false)))
                    {
                        p.SendMessage("You have been silenced for 1 minute. Reason: 3 warns (auto-mod, badwords)");
                        var mute = new Mute(p, "3 warnings (auto-mod, badwords)", new DateTime(0, 0, 0, 0, 1, 0));
                        mutelist.Add(p.Username);
                        mutes.Add(mute);
                    }
                    else
                    {
                        p.SendMessage($"Вы получили предупреждение! У Вас {this.warns[p.Username].ToString()} предупреждений всего. Причина: Запрещённые слова (авто-модерация)");
                    }

                    e.SetCancelled(true);
                }
            }
        }
        public void mutePlayer(NCore.NetworkPlayer player, Mute mute)
        {
            if (player == null) throw new NullReferenceException("Ссылка на объект не указывает на экземпляр объекта.");

            mutelist.Add(player.Username);
            mutes.Add(mute);
            player.Chat($"You have been muted until {mute.Mutetime.ToString()}. Reason: {mute.MuteReason}");
            Netcraft.Broadcast($"{player.Username} has been muted until {mute.Mutetime.ToString()}. Reason: {mute.MuteReason}");
        }
        public void unmutePlayer(NCore.NetworkPlayer player)
        {
            foreach (Mute m in mutes)
            {
                if (m.Player.UUID == player.UUID)
                {
                    if (!m.IsTimedOut())
                    {
                        player.Chat($"You have been unmuted.");
                        Netcraft.Broadcast($"{player.Username} has been unmuted.");

                        mutes.Remove(m);
                        break;
                    }
                }
            }
        }
        public bool isMuted(string nick)
        {
            foreach(Mute m in mutes)
            {
                if (m.Player.Username == nick)
                {
                    if (!m.IsTimedOut()) return true;
                }
            }
            return false;
        }
    }

    public class Mute
    {
        public NCore.NetworkPlayer Player { get; set; }
        public string MuteReason { get; set; }
        public DateTime Mutetime { get; set; }

        public Mute(NCore.NetworkPlayer a, string b, DateTime c)
        {
            Player = a;
            MuteReason = b;
            Mutetime = DateTime.Now.Add(c.TimeOfDay);
        }

        public bool IsTimedOut()
        {
            if (DateTime.Now > Mutetime)
                return true;
            return false;
        }
    }
}