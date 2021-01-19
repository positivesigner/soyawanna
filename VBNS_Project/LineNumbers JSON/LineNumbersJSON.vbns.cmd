start "" C:\TJBF\zPortableInstalls\VBNetScript\MxClasses\VBNetScript.exe /path %0 /source_data %1
Dim LINCTR = 0
Dim strSOURCE_FILE = Split_Parm("source_data", System.Environment.CommandLine)
Dim strNEW_FILE = strSOURCE_FILE & ".json"
Dim straSOURCE_FILE_PATH = strSOURCE_FILE.Split("\"c)
Dim strNEW_CARD_NAME = straSOURCE_FILE_PATH(straSOURCE_FILE_PATH.Length - 1)
If strSOURCE_FILE = mt Then
	MsgBox("No source_data parameter found: " & System.Environment.CommandLine)
Else
	Using stmNEW_FILE = New System.IO.StreamWriter(path:=strNEW_FILE, append:=False, encoding:=New System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=False, throwOnInvalidBytes:=True))
		stmNEW_FILE.WriteLine("[{")
		stmNEW_FILE.WriteLine(qs & "created" & qs & ":" & qs & Now().ToString("yyyyMMddHHmmssfff") & qs & ",")
		stmNEW_FILE.WriteLine(qs & "modified" & qs & ":" & qs & Now().ToString("yyyyMMddHHmmssfff") & qs & ",")
		stmNEW_FILE.WriteLine(qs & "title" & qs & ":" & qs & strNEW_CARD_NAME & qs & ",")
		LINCTR += 1
		stmNEW_FILE.Write(qs & Int_To_LnName(LINCTR) & qs & ":" & qs)
		Using stmSOURCE_DATA = New System.IO.StreamReader(path:=strSOURCE_FILE, encoding:=New System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=False, throwOnInvalidBytes:=True))
			While stmSOURCE_DATA.EndOfStream = False
				Dim intENTRY = stmSOURCE_DATA.Read()
				If intENTRY = 13 Then
				ElseIf intENTRY = 10 Then
					stmNEW_FILE.WriteLine(qs & ",")
					LINCTR += 1
					stmNEW_FILE.Write(qs & Int_To_LnName(LINCTR) & qs & ":" & qs)
				
				Else 'intENTRY
					Dim chrENTRY = ChrW(intENTRY)
					Dim chrPREFIX = ""
					If chrENTRY = "\"c OrElse
					  chrENTRY = """"c
						stmNEW_FILE.Write("\" & chrENTRY)
					
					ElseIf intENTRY = 9 Then
						stmNEW_FILE.Write(Space(4))
						
					ElseIf intENTRY < 32 Or
					  intENTRY = 44 Then
						Dim strHEX = "\u" & Right(Space(4).Replace(s, "0") & Hex(intENTRY), 4)
						stmNEW_FILE.Write(strHEX)
					
					Else
						stmNEW_FILE.Write(chrENTRY)
					End If
				End If 'intENTRY
			End While 'stmSOURCE_DATA
		End Using 'stmSOURCE_DATA
		
		stmNEW_FILE.WriteLine(qs)
		stmNEW_FILE.WriteLine("}]")
	End Using 'stmNEW_FILE
End If 'strSOURCE_FILE

RetVal = strNEW_FILE
End Function 'RetVal

Const mt As String = ""
Const qs As String = """"
Const s As String = " "

Shared Function Int_To_LnName(ur_int As Integer) As String
	Dim strRET_NAME = ""
	Dim strINT = ur_int.ToString
	Dim intPREFIX = 100
	While Len(strINT) > 3
		intPREFIX += 1
		strRET_NAME = ChrW(intPREFIX) & Right(strINT, 3) & strRET_NAME
		strINT = Left(strINT, Len(strINT) - 3)
	End While
	
	intPREFIX += 1
	Dim strPREFIX = ChrW(intPREFIX).ToString
	If Len(strINT) = 2 Then
		strPREFIX &= "c"
	ElseIf Len(strINT) = 3 Then
		strPREFIX &= "d"
	End If
	
	strRET_NAME = strPREFIX & strINT & strRET_NAME
	
	Int_To_LnName = strRET_NAME
End Function 'Int_To_LnName

Shared Function Split_Parm(ur_parm_name As String, ur_command_line As String) As String
Dim retSOURCE_FILE = mt
Dim intFOUND_INDEX = InStr(ur_command_line, ur_parm_name)
If intFOUND_INDEX > 0 Then
	retSOURCE_FILE = Trim(Mid(ur_command_line, intFOUND_INDEX + ur_parm_name.length))
	If Left(retSOURCE_FILE, 1) = "=" Then
		retSOURCE_FILE = Trim(Mid(retSOURCE_FILE, 2))
	End If
	
	If Left(retSOURCE_FILE, 1) = qs AndAlso
	  Right(retSOURCE_FILE, 1) = qs Then
		retSOURCE_FILE = Mid(retSOURCE_FILE, Len(retSOURCE_FILE) - 2)
	End If
End If

Split_Parm = retSOURCE_FILE
End Function 'Split_Parm
