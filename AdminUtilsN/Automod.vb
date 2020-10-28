Imports System.Text.RegularExpressions
Imports NCore.netcraft.server.api
Public Class Automod
    Dim warns As New Hashtable
    Dim mutes As List(Of Mute) = New List(Of Mute)
    Dim mutelist As Specialized.StringCollection = New Specialized.StringCollection
    Sub New()
        AddHandler NCSApi.PlayerChatEvent, AddressOf OnPlayerChat
    End Sub
    Const AUTOMOD_SWEAR_REGEX_MATCH As String = "\b((у|[нз]а|(хитро|не)?вз?[ыьъ]|с[ьъ]|(и|ра)[зс]ъ?|(о[тб]|под)[ьъ]?|(.\B)+?[оаеи])?-?([её]б(?!о[рй])|и[пб][ае][тц]).*?|(н[иеа]|[дп]о|ра[зс]|з?а|с(ме)?|о(т|дно)?)?-?ху([яйиеёю]|ли(?!ган)).*?|(в[зы]|(три|два|четыре)жды|(н|сук)а)?-?бл(я(?!(х|ш[кн]|мб)[ауеыио]).*?|[еэ][дт]ь?)|(ра[сз]|[зн]а|[со]|вы?|п(р[ои]|од)|и[зс]ъ?|[ао]т)?п[иеё]зд.*?|(за)?п[ие]д[аое]?р((ас)?(и(ли)?[нщктл]ь?)?|(о(ч[еи])?)?к|юг)[ауеы]?|манд([ауеы]|ой|[ао]вошь?(е?к[ауе])?|юк(ов|[ауи])?)|муд([аио].*?|е?н([ьюия]|ей))|([нз]а|по)х|мля([тд]ь)?|м[ао]л[ао]фь[яию])\b"
    Sub OnPlayerChat(e As events.PlayerChatEventArgs)
        If Not e.GetMessage.StartsWith("/") Then
            Dim m = e.GetMessage
            Dim p = e.GetPlayer
            If mutelist.Contains(p.Username) Then
                Dim mute As Mute = Nothing
                For Each mute1 In mutes
                    If mute1.Player.UUID = p.UUID Then
                        mute = mute1
                    End If
                Next
                If mute IsNot Nothing Then
                    If Not mute.IsTimedOut Then
                        e.SetCancelled(True)
                        p.SendMessage($"You have been silenced until {mute.Mutetime.ToString}: {mute.MuteReason}")
                    End If
                End If
            End If
            If Regex.Match(m, AUTOMOD_SWEAR_REGEX_MATCH, RegexOptions.IgnoreCase).Success Then
                If Not warns.Keys.Cast(Of String).ToArray.Contains(p.Username) Then
                    warns.Add(p.Username, 1)
                Else
                    warns.Add(p.Username, warns(p.Username) + 1)
                End If
                Plugin.GetInstance.AdminMessage($"[AU] {p.Username} got warned for [Badwords]. He has {warns(p.Username).ToString} warns.")
                Dim w = warns(p.Username)
                If w = 3 Then
                    p.SendMessage("You have been silenced for 1 minute. Reason: 3 warns (auto-mod, badwords)")
                    Dim mute = New Mute(p, "3 warnings (auto-mod, badwords)", New Date(0, 0, 0, 0, 1, 0))
                    mutelist.Add(p.Username)
                    mutes.Add(mute)
                Else
                    p.SendMessage($"Вы получили предупреждение! У Вас {warns(p.Username).ToString} предупреждений всего. Причина: Запрещённые слова (авто-модерация)")
                End If
                e.SetCancelled(True)
            End If
        End If
    End Sub
End Class
Public Class Mute
    Property Player As NCore.NetworkPlayer
    Property MuteReason As String
    Property Mutetime As Date
    Sub New(a As NCore.NetworkPlayer, b As String, c As Date)
        Player = a
        MuteReason = b
        Mutetime = Date.Now + c
    End Sub
    Function IsTimedOut()
        If Date.Now > Mutetime Then Return True
        Return False
    End Function
End Class