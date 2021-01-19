Namespace Mx
	Public Class mb
		Public Shared FirstExec As Object
		Public Shared uinput As Microsoft.VisualBasic.MsgBoxResult

		<System.Diagnostics.DebuggerHidden()>
		Public Shared Sub ask(ur_text As String)
			Call prv.Connect()
			If mb.uinput <> MsgBoxResult.Cancel Then
				mb.uinput = Microsoft.VisualBasic.MsgBox(ur_text, MsgBoxStyle.OkCancel, "")
			End If
		End Sub 'ask

		<System.Diagnostics.DebuggerHidden()>
		Public Shared Function GetText(ur_question As String) As String
			GetText = Microsoft.VisualBasic.InputBox(ur_question, "")
		End Function

		<System.Diagnostics.DebuggerHidden()>
		Public Shared Sub reset()
			Call prv.Connect()
			mb.uinput = MsgBoxResult.Ok
		End Sub

		Private Class prv
			<System.Diagnostics.DebuggerHidden()>
			Public Shared Sub Connect()
				If mb.FirstExec Is Nothing Then
					mb.uinput = MsgBoxResult.Ok
					mb.FirstExec = "done"
				End If
			End Sub 'Connect
		End Class 'prv
	End Class 'mb

	Public Class UserAction
		Public Shared Function Roman_Numeral_Conversion_Report() As String
			mb.reset()
			Dim strRET_MSG = ""
			Dim strNUMERAL = mb.GetText("Enter a Roman numeral")
			Dim rmnNUMERAL = New Mx.glblRomanNumeral(strNUMERAL)
			strRET_MSG = rmnNUMERAL.Validation_Message
			If strRET_MSG = "" Then
				Dim strDECIMAL = rmnNUMERAL.ToInteger.ToString
				strRET_MSG = strNUMERAL & " rom. = " & strDECIMAL & " dec."
			End If

			Roman_Numeral_Conversion_Report = strRET_MSG
		End Function 'Roman_Numeral_Conversion_Report
	End Class 'UserAction

	Public Class Assistant
	End Class 'Assistant

	Public Class glblRomanNumeral
		Private strpNumeral As String

		Private Enum enmLH
			low_or_high_number
			high_number
			low_number
		End Enum

		Private siaSymbol_Size As Mx.sia

		<System.Diagnostics.DebuggerHidden()>
		Public Sub New(ur_numeral As String)
			Me.strpNumeral = ur_numeral.Trim.ToUpper
			Me.siaSymbol_Size = prv.glbl_Symbol_Sizes()
		End Sub

		Public ReadOnly Property Numeral As String
			<System.Diagnostics.DebuggerHidden()>
			Get
				Numeral = Me.strpNumeral
			End Get
		End Property 'Numeral

		Public Function IsValid() As Boolean
			IsValid = (Me.Validation_Message = "")
		End Function

		Public Function Validation_Message() As String
			Dim symbol_size_table = Me.siaSymbol_Size
			Dim strBASE = Me.strpNumeral
			Dim clrRET_STATE = New prv.Loop_Valid

			For CHRCTR = 1 To strBASE.Length
				Dim strCUR_CHAR = Mid(strBASE, CHRCTR, 1)
				clrRET_STATE = prv.Loop_Return_Valid(clrRET_STATE, strCUR_CHAR, symbol_size_table)
			Next CHRCTR

			clrRET_STATE = prv.Loop_Return_Valid(clrRET_STATE, "", symbol_size_table)

			Validation_Message = clrRET_STATE.strNOTICE_MSG
		End Function 'Validation_Message

		Public Function ToInteger() As Integer
			Dim symbol_size_table = Me.siaSymbol_Size
			Dim strBASE = Me.strpNumeral
			Dim clrRET_STATE = New prv.Loop_State
			For CHRCTR = 1 To strBASE.Length
				Dim strCUR_CHAR = Mid(strBASE, CHRCTR, 1)
				Dim clPREV_STATE = clrRET_STATE
				clrRET_STATE = prv.Loop_Return_State(clrRET_STATE, strCUR_CHAR, symbol_size_table)
			Next CHRCTR

			clrRET_STATE = prv.Loop_Return_State(clrRET_STATE, "", symbol_size_table)

			ToInteger = clrRET_STATE.intRET_NUM
		End Function 'ToInteger

		Private Class prv
			<System.Diagnostics.DebuggerHidden()>
			Public Shared Function glbl_Symbol_Sizes() As Mx.sia
				Dim retSIA = New Mx.sia
				glbl_Symbol_Sizes = retSIA
				retSIA.Add("I", 1)
				retSIA.Add("V", 5)
				retSIA.Add("X", 10)
				retSIA.Add("L", 50)
				retSIA.Add("C", 100)
				retSIA.Add("D", 500)
				retSIA.Add("M", 1000)

				glbl_Symbol_Sizes = retSIA
			End Function 'glbl_Symbol_Sizes

			Public Class Loop_State
				Public enrSTATE As enmLH
				Public strLOW_OR_HIGH_TEMP As String
				Public intRET_NUM As Integer

				<System.Diagnostics.DebuggerHidden()>
				Public Sub New()
					Me.enrSTATE = enmLH.high_number
					Me.strLOW_OR_HIGH_TEMP = ""
					Me.intRET_NUM = 0
				End Sub 'New
			End Class 'Loop_State

			Public Class Loop_Valid
				Public strVAL_FOUR As String
				Public strVAL_THREE As String
				Public strVAL_TWO As String
				Public strVAL_ONE As String
				Public intVAL_FOUR As String
				Public intVAL_THREE As String
				Public intVAL_TWO As String
				Public intVAL_ONE As String
				Public strNOTICE_MSG As String

				<System.Diagnostics.DebuggerHidden()>
				Public Sub New()
					Call prvLoop_Valid.Reset_Symobls(Me)
					Me.strNOTICE_MSG = ""
				End Sub 'New

				<System.Diagnostics.DebuggerHidden()>
				Public Sub New(ur_state As prv.Loop_Valid, ur_new_symbol As String, ur_symbol_size As Mx.sia)
					Me.strVAL_FOUR = ur_state.strVAL_THREE
					Me.intVAL_FOUR = ur_state.intVAL_THREE
					Me.strVAL_THREE = ur_state.strVAL_TWO
					Me.intVAL_THREE = ur_state.intVAL_TWO
					Me.strVAL_TWO = ur_state.strVAL_ONE
					Me.intVAL_TWO = ur_state.intVAL_ONE
					Me.strVAL_ONE = ur_new_symbol
					Me.intVAL_ONE = prv.Numeral_To_Int(ur_symbol_size, ur_new_symbol)
					Me.strNOTICE_MSG = ur_state.strNOTICE_MSG
				End Sub 'New

				<System.Diagnostics.DebuggerHidden()>
				Public Sub Assign_NoticeMsg(ur_message As String)
					If strNOTICE_MSG <> "" Then
						strNOTICE_MSG &= vbCrLf & vbCrLf
					End If

					strNOTICE_MSG &= ur_message
					Call prvLoop_Valid.Reset_Symobls(Me)
				End Sub 'Assign_NoticeMsg

				Private Class prvLoop_Valid
					Public Shared Sub Reset_Symobls(ur_state As prv.Loop_Valid)
						ur_state.strVAL_FOUR = ""
						ur_state.intVAL_FOUR = 0
						ur_state.strVAL_THREE = ""
						ur_state.intVAL_THREE = 0
						ur_state.strVAL_TWO = ""
						ur_state.intVAL_TWO = 0
						ur_state.strVAL_ONE = ""
						ur_state.intVAL_ONE = 0
					End Sub 'Reset_Symobls
				End Class 'prv
			End Class 'Loop_Valid

			Public Shared Function Loop_Return_Valid(ur_state As prv.Loop_Valid, ur_new_symbol As String, ur_symbol_size As Mx.sia) As prv.Loop_Valid
				Dim retSTATE = New prv.Loop_Valid(ur_state, ur_new_symbol, ur_symbol_size)
				Loop_Return_Valid = retSTATE

				If retSTATE.strVAL_ONE <> "" Then
					If retSTATE.intVAL_ONE = 0 Then
						Dim strLIST = ""
						For Each strENTRY In ur_symbol_size.Keys
							If strLIST <> "" Then
								strLIST &= ", "
							End If

							strLIST &= strENTRY
						Next strENTRY

						retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_ONE & "] is not valid: Only the symbols [" & strLIST & "] may be used in Roman numerals.")

					ElseIf retSTATE.strVAL_FOUR = retSTATE.strVAL_THREE AndAlso
					  retSTATE.strVAL_FOUR = retSTATE.strVAL_TWO AndAlso
					  retSTATE.strVAL_FOUR = retSTATE.strVAL_ONE Then
						retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_FOUR & retSTATE.strVAL_THREE & retSTATE.strVAL_TWO & retSTATE.strVAL_ONE & ur_new_symbol & "] is not valid: A single symbol may not be repeated more than three times.")

					ElseIf retSTATE.strVAL_TWO = retSTATE.strVAL_ONE AndAlso
					  Left(retSTATE.intVAL_ONE.ToString, 1) <> "1" Then
						Dim strLIST = ""
						For Each strENTRY In ur_symbol_size.Keys
							Dim strENTRY_VAL = prv.Numeral_To_Int(ur_symbol_size, strENTRY).ToString
							If Left(strENTRY_VAL, 1) = "1" Then
								If strLIST <> "" Then
									strLIST &= ", "
								End If

								strLIST &= strENTRY
							End If
						Next strENTRY

						retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_TWO & retSTATE.strVAL_ONE & "] is not valid: Only the symbols [" & strLIST & "] may be repeated.")

					ElseIf retSTATE.strVAL_TWO <> "" Then

						If retSTATE.intVAL_ONE > retSTATE.intVAL_TWO AndAlso
						  retSTATE.intVAL_ONE <> retSTATE.intVAL_TWO * 10 Then
							retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_TWO & retSTATE.strVAL_ONE & "] is not valid: The lower symbol must be ten times less value than the following higher symbol.")

						ElseIf retSTATE.intVAL_ONE > retSTATE.intVAL_TWO AndAlso
						  Left(retSTATE.intVAL_ONE.ToString, 1) <> "1" Then
							Dim strLIST = ""
							For Each strENTRY In ur_symbol_size.Keys
								Dim strENTRY_VAL = prv.Numeral_To_Int(ur_symbol_size, strENTRY).ToString
								If Left(strENTRY_VAL, 1) = "1" Then
									If strLIST <> "" Then
										strLIST &= ", "
									End If

									strLIST &= strENTRY
								End If
							Next strENTRY

							retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_TWO & retSTATE.strVAL_ONE & "] is not valid: Only the symbols [" & strLIST & "] may be used as the lower symbol followed by a higher symbol.")

						ElseIf retSTATE.strVAL_THREE <> "" Then
							If retSTATE.intVAL_TWO > retSTATE.intVAL_ONE AndAlso
							  retSTATE.intVAL_ONE >= retSTATE.intVAL_THREE Then
								retSTATE.Assign_NoticeMsg("[" & retSTATE.strVAL_THREE & retSTATE.strVAL_TWO & retSTATE.strVAL_ONE & "] is not valid: A lower symbol followed by higher symbol must be followed by lower symbol than the first of the series.")
							End If
						End If 'strVAL_THREE
					End If 'strVAL_TWO
				End If 'strVAL_ONE
			End Function 'Loop_Return_Valid

			Public Shared Function Loop_Return_State(ur_state As prv.Loop_State, ur_new_symbol As String, ur_symbol_size As Mx.sia) As prv.Loop_State
				Dim retSTATE = New prv.Loop_State
				Loop_Return_State = retSTATE

				Dim intNEW_VALUE = prv.Numeral_To_Int(ur_symbol_size, ur_new_symbol)
				Dim intLOW_OR_HIGH_VALUE = prv.Numeral_To_Int(ur_symbol_size, ur_state.strLOW_OR_HIGH_TEMP)
				If ur_state.strLOW_OR_HIGH_TEMP = "" Then
					retSTATE.intRET_NUM = ur_state.intRET_NUM
					retSTATE.enrSTATE = enmLH.low_or_high_number
					retSTATE.strLOW_OR_HIGH_TEMP = ur_new_symbol

				ElseIf intNEW_VALUE > intLOW_OR_HIGH_VALUE Then
					retSTATE.intRET_NUM = ur_state.intRET_NUM + intNEW_VALUE - intLOW_OR_HIGH_VALUE
					retSTATE.enrSTATE = enmLH.high_number
					retSTATE.strLOW_OR_HIGH_TEMP = ""

				Else
					retSTATE.intRET_NUM = ur_state.intRET_NUM + intLOW_OR_HIGH_VALUE
					retSTATE.enrSTATE = enmLH.low_number
					retSTATE.strLOW_OR_HIGH_TEMP = ur_new_symbol
				End If
			End Function 'Loop_Return_State

			<System.Diagnostics.DebuggerHidden()>
			Public Shared Function Numeral_To_Int(ur_symbol_size As Mx.sia, ur_numeral As String) As Integer
				Numeral_To_Int = 0
				If ur_symbol_size.ContainsKey(ur_numeral) Then
					Numeral_To_Int = ur_symbol_size.Item(ur_numeral)
				End If
			End Function 'Numeral_To_Int
		End Class 'prv
	End Class 'glblRomanNumeral

	Public Class sia
		Inherits System.Collections.Generic.Dictionary(Of String, Integer)
	End Class
End Namespace 'Mx
