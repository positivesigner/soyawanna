Option Strict On
Namespace Mx
    Public Module ConstVar
        Public Const mt = ""
        Public Const qs = """"
        Public Const qt = "'"
        Public Const s = " "

        <System.Diagnostics.DebuggerHidden()> Public Function AreEqual(ur_val As String, ur_cmp As String) As Boolean
            AreEqual = String.Equals(ur_val, ur_cmp, System.StringComparison.CurrentCultureIgnoreCase)
        End Function
        <System.Diagnostics.DebuggerHidden()> Public Function b0(ur_val As Integer) As Integer
            b0 = ur_val - 1
        End Function
        <System.Diagnostics.DebuggerHidden()> Public Function b1(ur_val As Integer) As Integer
            b1 = ur_val + 1
        End Function
        <System.Diagnostics.DebuggerHidden()> Public Function ContainingText(ur_large_text As String, ur_small_text As String) As Boolean
            ContainingText = InStr(ur_large_text, ur_small_text, CompareMethod.Text) > 0
        End Function 'ContainingText
        <System.Diagnostics.DebuggerHidden()> Public Function gUTF8_FileEncoding() As System.Text.UTF8Encoding
            gUTF8_FileEncoding = glbl.gUTF8_Encoding
        End Function
        <System.Diagnostics.DebuggerHidden()> Public Function EndingWithText(ur_large_text As String, ur_small_text As String) As Boolean
            EndingWithText = AreEqual(Right(ur_large_text, Len(ur_small_text)), ur_small_text)
        End Function 'EndingWithText
        <System.Diagnostics.DebuggerHidden()> Public Function HasText(ur_value As String) As Boolean
            HasText = Not String.IsNullOrWhiteSpace(ur_value)
        End Function 'HasText
        <System.Diagnostics.DebuggerHidden()> Public Function StartingWithText(ur_large_text As String, ur_small_text As String) As Boolean
            StartingWithText = AreEqual(Left(ur_large_text, Len(ur_small_text)), ur_small_text)
        End Function 'StartingWithText

        <System.Diagnostics.DebuggerHidden()> Public Function CmdOutput(ur_exec_path As String, Optional ur_exec_param As String = mt) As String
            Dim objPCS As New System.Diagnostics.Process()
            With 1 : Dim prcINFO = objPCS.StartInfo
                prcINFO.FileName = ur_exec_path
                prcINFO.Arguments = ur_exec_param
                prcINFO.UseShellExecute = False
                prcINFO.RedirectStandardOutput = True
                prcINFO.CreateNoWindow = True
            End With 'prcINFO

            objPCS.Start()
            CmdOutput = objPCS.StandardOutput.ReadToEnd()
            objPCS.WaitForExit()
        End Function 'CmdOutput

        <System.Diagnostics.DebuggerHidden()> Public Function Strapd() As Strap
            Strapd = New Strap
        End Function

        Public Class glbl
            Private Shared objUTF8_ENCODING As System.Text.UTF8Encoding

            <System.Diagnostics.DebuggerHidden()> Private Shared Sub Init()
                If objUTF8_ENCODING Is Nothing Then
                    objUTF8_ENCODING = New System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=False, throwOnInvalidBytes:=True)
                End If
            End Sub 'Init

            <System.Diagnostics.DebuggerHidden()> Public Shared Function gUTF8_Encoding() As System.Text.UTF8Encoding
                Call glbl.Init()
                gUTF8_Encoding = glbl.objUTF8_ENCODING
            End Function
        End Class 'glbl
    End Module 'ConstVar

    Public Class ErrListBase
        Dim pstpNOTICE_MSG As Strap

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            pstpNOTICE_MSG = Strapd()
        End Sub

        <System.Diagnostics.DebuggerHidden()> Public Function DoContinue() As Boolean
            DoContinue = (Me.Found = False)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Sub dError_Data(ur_exception As System.Exception, ur_methodbase As System.Reflection.MethodBase, Optional ur_procedure_status As String = mt)
            If pstpNOTICE_MSG.Length > 0 Then
                pstpNOTICE_MSG.dLine().dLine()
            End If

            pstpNOTICE_MSG.dLine(ur_exception.Message)
            pstpNOTICE_MSG.dLine(Strapd().d("Error in @r1.@r2").r1(ur_methodbase.DeclaringType.Name).r2(ur_methodbase.Name).dS("(status: @r1)").r1(ur_procedure_status))
        End Sub 'dError_Data

        <System.Diagnostics.DebuggerHidden()> Public Sub dError_Stack(ur_exception As System.Exception)
            Dim hr As Integer = System.Runtime.InteropServices.Marshal.GetHRForException(ur_exception)
            pstpNOTICE_MSG.dLine(ur_exception.GetType.ToString & "(0x" & hr.ToString("X8") & "): " & ur_exception.Message & System.Environment.NewLine & ur_exception.StackTrace & System.Environment.NewLine)
            Dim st = New System.Diagnostics.StackTrace(ur_exception, True)
            For Each objFRAME In st.GetFrames
                If objFRAME.GetFileLineNumber() > 0 Then
                    pstpNOTICE_MSG.dLine("Line:" & objFRAME.GetFileLineNumber() & " Filename: " & System.IO.Path.GetFileName(objFRAME.GetFileName) & System.Environment.NewLine)
                End If
            Next objFRAME
        End Sub 'dError_Stack

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function Found() As Boolean
            Found = (pstpNOTICE_MSG.Length > 0)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function ToString() As String
            ToString = pstpNOTICE_MSG
        End Function 'ToString

        <System.Diagnostics.DebuggerHidden()> Public Sub Throw_Error_Data(ur_exception As System.Exception, ur_methodbase As System.Reflection.MethodBase, Optional ur_procedure_status As String = mt)
            Call Me.dError_Data(ur_exception, ur_methodbase, ur_procedure_status)
            Throw New LocalException(mt)
        End Sub 'Throw_Error_Data


        Public Class LocalException
            Inherits System.Exception

            <System.Diagnostics.DebuggerHidden()> Public Sub New()
            End Sub

            <System.Diagnostics.DebuggerHidden()> Public Sub New(message As String)
                MyBase.New(message)
            End Sub

            <System.Diagnostics.DebuggerHidden()> Public Sub New(message As String, inner As System.Exception)
                MyBase.New(message, inner)
            End Sub
        End Class 'LocalException
    End Class 'ErrListBase

    Public Class ObaCTR(Of T)
        Inherits RCTR

        Private vsdaLIST As Obalist(Of T)

        <System.Diagnostics.DebuggerHidden()> Public Sub New(Optional ur_list As Obalist(Of T) = Nothing)
            Call MyBase.New(prv.TotalItems(ur_list))
            Me.vsdaLIST = ur_list
        End Sub

        Public Shadows ReadOnly Property Current() As ObaCTR(Of T)
            <System.Diagnostics.DebuggerHidden()> Get
                Current = Me
            End Get
        End Property

        Public Shadows ReadOnly Property v() As T
            <System.Diagnostics.DebuggerHidden()> Get
                v = Me.vsdaLIST.Item(Me.Indexenm)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function GetEnumerator() As ObaCTR(Of T)
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Private Class prv
            <System.Diagnostics.DebuggerHidden()> Public Shared Function TotalItems(Optional ur_list As Obalist(Of T) = Nothing) As Integer
                TotalItems = 0
                If ur_list IsNot Nothing Then
                    TotalItems = ur_list.Count
                End If
            End Function 'TotalItems
        End Class 'prv
    End Class 'ObaCTR

    Public Class ObjCTR(Of T)
        Inherits RCTR

        Private vsdaLIST As Objlist(Of T)

        <System.Diagnostics.DebuggerHidden()> Public Sub New(ur_list As Objlist(Of T))
            Call MyBase.New(ur_list.Count)
            Me.vsdaLIST = ur_list
        End Sub

        Public Shadows ReadOnly Property Current() As ObjCTR(Of T)
            <System.Diagnostics.DebuggerHidden()> Get
                Current = Me
            End Get
        End Property

        Public Shadows ReadOnly Property row() As T
            <System.Diagnostics.DebuggerHidden()> Get
                row = Me.vsdaLIST.Item(Me.Indexenm)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function GetEnumerator() As ObjCTR(Of T)
            GetEnumerator = Me
            Call Me.Reset()
        End Function
    End Class 'ObjCTR

    Public Class Obalist(Of T)
        Private vobjLIST() As T

        <System.Diagnostics.DebuggerHidden()> Public Sub New(Optional ur_list() As T = Nothing)
            Me.vobjLIST = ur_list
        End Sub

        Public ReadOnly Property Count() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                Count = Me.vobjLIST.Length
            End Get
        End Property

        Public Property Item(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()> Get
                Item = Me.vobjLIST(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()> Set(ur_val As T)
                Me.vobjLIST(ur_index) = ur_val
            End Set
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Function kvp() As ObaCTR(Of T)
            kvp = New ObaCTR(Of T)(Me)
        End Function
    End Class 'Obalist

    Public Class Objlist(Of T)
        Inherits System.Collections.Generic.List(Of T)

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function kvp() As ObjCTR(Of T)
            kvp = New ObjCTR(Of T)(Me)
        End Function
    End Class 'Objlist

    Public Class RCTR
        Private pintMAX_NUM As Integer
        Private pintCUR_NUM As Integer

        <System.Diagnostics.DebuggerHidden()> Public Shared Widening Operator CType(ByVal b As RCTR) As Integer
            Return b.Indexb1
        End Operator 'CType

        <System.Diagnostics.DebuggerHidden()> Public Sub New(ur_max_num As Integer)
            Me.pintMAX_NUM = ur_max_num
        End Sub

        Public Shadows ReadOnly Property Current() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                Current = Me.pintCUR_NUM
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Function GetEnumerator() As RCTR
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Public ReadOnly Property Indexb1() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                Indexb1 = Me.pintCUR_NUM
            End Get
        End Property

        Public ReadOnly Property Indexenm() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                Indexenm = b0(Me.pintCUR_NUM)
            End Get
        End Property

        Public ReadOnly Property LastIndexb1() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                LastIndexb1 = Me.pintMAX_NUM
            End Get
        End Property

        Public ReadOnly Property LastIndexenm() As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                LastIndexenm = b0(Me.pintMAX_NUM)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Function MoveNext() As Boolean
            MoveNext = (Me.pintCUR_NUM < Me.pintMAX_NUM)
            Me.pintCUR_NUM += 1
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function Reset() As RCTR
            Reset = Me
            Me.pintCUR_NUM = 0
        End Function
    End Class 'RCTR

    Public Class SDACTR
        Inherits RCTR

        Private psdaLIST As Sdata

        <System.Diagnostics.DebuggerHidden()> Public Sub New(ur_sdata As Sdata)
            Call MyBase.New(ur_sdata.Count)
            Me.psdaLIST = ur_sdata
        End Sub

        Public Shadows ReadOnly Property Current() As SDACTR
            <System.Diagnostics.DebuggerHidden()> Get
                Current = Me
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function GetEnumerator() As SDACTR
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Public Shadows ReadOnly Property v() As String
            <System.Diagnostics.DebuggerHidden()> Get
                v = Me.psdaLIST.Item(Me.Indexenm)
            End Get
        End Property
    End Class 'SDACTR

    Public Class Sdata
        Inherits System.Collections.Generic.List(Of String)

        <System.Diagnostics.DebuggerHidden()> Public Function d(ur_val As String) As Sdata
            d = Me
            Call Me.Add(ur_val)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dList(ParamArray ur_val() As String) As Sdata
            dList = Me
            If ur_val IsNot Nothing Then
                For Each strENTRY In ur_val
                    Call Me.Add(strENTRY)
                Next strENTRY
            End If
        End Function 'dList

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function gCopy() As Sdata
            gCopy = New Sdata().dList(Me.ToArray)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function kvp() As SDACTR
            kvp = New SDACTR(Me)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Shared Function Split(ur_text As String, ur_sprtr As Char) As Sdata
            Dim sdaCMP = New Sdata
            Split = sdaCMP
            For Each strTEXT In ur_text.Split(ur_sprtr)
                sdaCMP.Add(strTEXT)
            Next
        End Function 'Split

        Public Property v_b1(ur_index As Integer) As String
            <System.Diagnostics.DebuggerHidden()> Get
                v_b1 = Me.Item(b0(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()> Set(ur_val As String)
                Me.Item(b0(ur_index)) = ur_val
            End Set
        End Property

        Public Property v_enm(ur_index As Integer) As String
            <System.Diagnostics.DebuggerHidden()> Get
                v_enm = Me.Item(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()> Set(ur_val As String)
                Me.Item(ur_index) = ur_val
            End Set
        End Property
    End Class 'Sdata

    Public Class Strap
        Private pstbTEXT As System.Text.StringBuilder

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            Me.pstbTEXT = New System.Text.StringBuilder
        End Sub

        <System.Diagnostics.DebuggerHidden()> Public Shared Widening Operator CType(b As Strap) As String
            Return b.ToString
        End Operator

        <System.Diagnostics.DebuggerHidden()> Public Function Clear() As Strap
            Clear = Me
            Call Me.pstbTEXT.Clear()
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function d(ur_text As String) As Strap
            d = Me.dSprtr(mt, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dCSV_Quoted(ur_index_b1 As Integer, ur_text As String) As Strap
            dCSV_Quoted = Me
            If ur_index_b1 > 1 Then
                Me.d(",")
            End If

            Me.d(qs).d(ur_text.Replace(qs, qs & qs)).d(qs)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dList(ur_sprtr As String, ParamArray ur_text() As String) As Strap
            dList = Me
            If ur_text IsNot Nothing Then
                For Each strENTRY In ur_text
                    Call Me.dSprtr(ur_sprtr, strENTRY)
                Next strENTRY
            End If
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dCSVList(ParamArray ur_text() As String) As Strap
            dCSVList = Me
            If ur_text IsNot Nothing Then
                For ENTCTR = 0 To UBound(ur_text)
                    Me.dCSV_Quoted(ENTCTR + 1, ur_text(ENTCTR))
                Next ENTCTR
            End If
        End Function 'dCSVList

        <System.Diagnostics.DebuggerHidden()> Public Function dLine(Optional ur_text As String = "") As Strap
            dLine = Me.dSprtr(vbCrLf, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dS(ur_text As String) As Strap
            dS = Me.dSprtr(s, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function dSprtr(ur_sprtr As String, ur_text As String) As Strap
            dSprtr = Me
            Me.pstbTEXT.Append(ur_sprtr).Append(ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function gCopy(ur_text As String) As Strap
            gCopy = New Strap().d(Me)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function HasText() As Boolean
            HasText = (Me.Length > 0)
        End Function

        Public ReadOnly Property Length As Integer
            <System.Diagnostics.DebuggerHidden()> Get
                Length = Me.pstbTEXT.Length
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()> Public Function r1(ur_text As String) As Strap
            r1 = Me
            Call Me.pstbTEXT.Replace("@r1", ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function r2(ur_text As String) As Strap
            r2 = Me
            Call Me.pstbTEXT.Replace("@r2", ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function rx(ur_index As Integer, ur_text As String) As Strap
            rx = Me
            Call Me.pstbTEXT.Replace("@r" & ur_index, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function ToString() As String
            ToString = Me.pstbTEXT.ToString
        End Function
    End Class 'Strap

    Public Class TRow(Of T)
        Inherits Sdata

        <System.Diagnostics.DebuggerHidden()> Public Sub New()
            For Each intVAL In Me.RefColNames
                Me.Add(mt)
            Next
        End Sub 'New

        <System.Diagnostics.DebuggerHidden()> Public Shadows Function gCopy() As TRow(Of T)
            Dim sdaRET = New TRow(Of T)
            gCopy = sdaRET
            For Each kvpENTRY In Me.kvp
                sdaRET.v_enm(kvpENTRY.Indexenm) = kvpENTRY.v
            Next kvpENTRY
        End Function

        <System.Diagnostics.DebuggerHidden()> Public Function RefColNames() As Sdata
            RefColNames = TRow(Of T).glbl.RefNames
        End Function

        Public Property v(ur_cl As T) As String
            <System.Diagnostics.DebuggerHidden()> Get
                v = Me.Item(DirectCast(CObj(ur_cl), Integer))
            End Get
            <System.Diagnostics.DebuggerHidden()> Set(ur_val As String)
                Me.Item(DirectCast(CObj(ur_cl), Integer)) = ur_val
            End Set
        End Property

        Public Class glbl
            Private Shared sdaTCOL_NAME As Sdata
            Private Shared sdjTCOL_KEY As Objlist(Of T)

            <System.Diagnostics.DebuggerHidden()> Private Shared Sub Init()
                If sdaTCOL_NAME Is Nothing Then
                    sdaTCOL_NAME = New Sdata
                    sdjTCOL_KEY = New Objlist(Of T)
                    For Each intVAL In System.Enum.GetValues(GetType(T))
                        Dim enmCAST = DirectCast(CObj(intVAL), T)
                        sdaTCOL_NAME.Add(intVAL.ToString.ToLower)
                        sdjTCOL_KEY.Add(enmCAST)
                    Next
                End If 'sdaTCOL_NAME
            End Sub 'Init

            <System.Diagnostics.DebuggerHidden()> Public Shared Function RefKeys() As Objlist(Of T)
                Call glbl.Init()
                RefKeys = glbl.sdjTCOL_KEY
            End Function

            <System.Diagnostics.DebuggerHidden()> Public Shared Function RefNames() As Sdata
                Call glbl.Init()
                RefNames = glbl.sdaTCOL_NAME
            End Function
        End Class 'glbl
    End Class 'Trow

    Partial Public Class MxText 'Parse
        Public Class ResultSplit
            Public Key As String
            Public RemText As String
            Public SearchForSprtr As String
            <System.Diagnostics.DebuggerHidden()> Public Sub New()
                Me.Key = mt
                Me.RemText = mt
                Me.SearchForSprtr = mt
            End Sub 'New
        End Class 'ResultSplit

        <System.Diagnostics.DebuggerHidden()> Public Shared Function FirstSprtrSplit(ur_line As String, ur_separator() As Char) As ResultSplit
            Dim sdaCMP = New MxText.ResultSplit
            sdaCMP.SearchForSprtr = mt
            FirstSprtrSplit = sdaCMP
            Dim arrCHAR = ur_line.ToCharArray
            Dim intPARSE_STATE = prv.enmSPRTR_STATE.start
            Dim intLAST_CHAR = UBound(arrCHAR)
            For CHRCTR = 0 To intLAST_CHAR
                Dim curCHR = arrCHAR(CHRCTR)
                Select Case intPARSE_STATE
                    Case prv.enmSPRTR_STATE.start
                        Dim bolANY_SPRTR_MATCH = False
                        For Each curSPRTR In ur_separator
                            If curCHR = curSPRTR Then
                                bolANY_SPRTR_MATCH = True
                                Exit For
                            End If 'objCHR
                        Next curSPRTR

                        If bolANY_SPRTR_MATCH Then
                            Dim intSPRTR_AT = b1(CHRCTR)
                            sdaCMP.RemText = Trim(Mid(ur_line, intSPRTR_AT + 1))
                            If curCHR = s AndAlso
                               HasText(sdaCMP.RemText) Then
                                Dim nxtCHR = CChar(Left(sdaCMP.RemText, 1))
                                Dim bolANY_SECOND_SPRTR_MATCH = False
                                For Each curSPRTR In ur_separator
                                    If curSPRTR <> curCHR AndAlso
                                       curSPRTR = nxtCHR Then
                                        bolANY_SECOND_SPRTR_MATCH = True
                                        Exit For
                                    End If 'curSPRTR
                                Next curSPRTR

                                If bolANY_SECOND_SPRTR_MATCH Then
                                    curCHR = nxtCHR
                                    sdaCMP.RemText = Trim(Mid(sdaCMP.RemText, 2))
                                End If
                            End If 'curCHR

                            sdaCMP.Key = Trim(Mid(ur_line, 1, intSPRTR_AT - 1))
                            sdaCMP.SearchForSprtr = curCHR
                            Exit For

                        Else 'bolANY_SPRTR_MATCH
                            If curCHR = qs Then
                                intPARSE_STATE = prv.enmSPRTR_STATE.in_quotes
                            End If
                        End If 'objCHR

                    Case prv.enmSPRTR_STATE.in_quotes
                        If curCHR = qs Then
                            intPARSE_STATE = prv.enmSPRTR_STATE.start
                            Dim intNEXT_IX = CHRCTR + 1
                            If intNEXT_IX < intLAST_CHAR Then
                                If arrCHAR(intNEXT_IX) = qs Then
                                    intPARSE_STATE = prv.enmSPRTR_STATE.escaped_dbl_sprtr
                                End If 'intNEXT_IX
                            End If 'intNEXT_IX
                        End If 'objCHR

                    Case prv.enmSPRTR_STATE.escaped_dbl_sprtr
                        intPARSE_STATE = prv.enmSPRTR_STATE.in_quotes
                End Select 'intPARSE_STATE
            Next CHRCTR

            If HasText(sdaCMP.SearchForSprtr) = False Then
                sdaCMP.Key = ur_line
            End If 'sdaCMP
        End Function 'FirstSprtrSplit

        <System.Diagnostics.DebuggerHidden()> Public Shared Function Unquote(ur_line As String, Optional ur_sprtr As String = qs) As Strap
            Dim stpRET As New Strap
            Unquote = stpRET
            Dim arrCHAR = ur_line.ToCharArray
            Dim intPARSE_FROM = prv.enmSPRTR_STATE.start
            Dim intLAST_CHAR = UBound(arrCHAR)
            For CHRCTR = 0 To intLAST_CHAR
                Dim curCHAR = arrCHAR(CHRCTR)
                Select Case intPARSE_FROM
                    Case prv.enmSPRTR_STATE.start
                        If curCHAR = ur_sprtr Then
                            intPARSE_FROM = prv.enmSPRTR_STATE.in_quotes

                        Else 'objCHR
                            stpRET.d(curCHAR)
                        End If 'objCHR

                    Case prv.enmSPRTR_STATE.in_quotes
                        If curCHAR = ur_sprtr Then
                            intPARSE_FROM = prv.enmSPRTR_STATE.start
                            If CHRCTR < intLAST_CHAR AndAlso
                               arrCHAR(CHRCTR + 1) = ur_sprtr Then
                                intPARSE_FROM = prv.enmSPRTR_STATE.escaped_dbl_sprtr
                            End If

                        Else 'objCHR
                            stpRET.d(curCHAR)
                        End If

                    Case prv.enmSPRTR_STATE.escaped_dbl_sprtr
                        stpRET.d(curCHAR)
                        intPARSE_FROM = prv.enmSPRTR_STATE.in_quotes
                End Select 'intPARSE_STATE
            Next CHRCTR
        End Function 'Unquote

        Private Class prv
            Public Enum enmSPRTR_STATE
                start
                escaped_dbl_sprtr
                in_quotes
            End Enum 'enmSPRTR_STATE
        End Class 'prv
    End Class 'MxText-Parse

    Partial Public Class MxText 'Csv
        <System.Diagnostics.DebuggerHidden()> Public Shared Function Parse_Quotes(ur_command_line As String) As Sdata
            Dim sdaRET = New Sdata
            Parse_Quotes = sdaRET
            Dim objLINE = MxText.FirstSprtrSplit(ur_command_line, {" "c})
            While HasText(objLINE.RemText)
                Dim objSPLIT = MxText.FirstSprtrSplit(objLINE.RemText, {" "c})
                If HasText(objSPLIT.Key) Then
                    sdaRET.d(MxText.Unquote(objSPLIT.Key))
                End If

                If HasText(objSPLIT.RemText) = False Then
                    sdaRET.d(MxText.Unquote(objLINE.RemText))
                End If

                objLINE = objSPLIT
            End While 'objLINE
        End Function 'Parse_Quotes

        Public Class CmdRow(Of T)
            <System.Diagnostics.DebuggerHidden()> Public Shared Function Parse_Input(ur_command_line As String) As TRow(Of T)
                Parse_Input = Parse_Input(New TRow(Of T), ur_command_line)
            End Function 'Parse_Input

            <System.Diagnostics.DebuggerHidden()> Public Shared Function Parse_Input(ur_cmd_row As TRow(Of T), ur_command_line As String) As TRow(Of T)
                'System.Environment.CommandLine, System.AppDomain.CurrentDomain, System.Reflection.Assembly.GetExecutingAssembly
                Dim trwRET = ur_cmd_row
                Parse_Input = trwRET
                Dim objLINE = MxText.FirstSprtrSplit(ur_command_line, {"/"c}) 'System.Environment.CommandLine
                While HasText(objLINE.SearchForSprtr)
                    objLINE = MxText.FirstSprtrSplit(objLINE.RemText, {"/"c})
                    Dim objSPLIT = MxText.FirstSprtrSplit(objLINE.Key, {"="c, ":"c, " "c})
                    Dim intFOUND_KEY = trwRET.RefColNames.IndexOf((Trim(MxText.Unquote(objSPLIT.Key)).ToLower))
                    If intFOUND_KEY >= 0 Then
                        If HasText(objSPLIT.SearchForSprtr) Then
                            trwRET.v_enm(intFOUND_KEY) = MxText.Unquote(objSPLIT.RemText)
                        Else
                            trwRET.v_enm(intFOUND_KEY) = True.ToString
                        End If
                    End If 'olnFOUND_KEY
                End While 'objLINE
            End Function 'Parse_Input
        End Class 'CmdRow
    End Class 'MxText-CommandParm

    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()> Public Shared Function CSVSplit(ByVal ur_line As String, Optional ur_separator As String = ",") As Sdata
            Dim sdaCMP = New Sdata
            CSVSplit = sdaCMP
            Dim objARRAY = ur_line.ToCharArray
            Dim objSPRTR = Mid(ur_separator, 1, 1).ToCharArray()(0)
            Dim objESCAPE_QUOTE = """"c

            Const cstSTART = 0
            Const cstESC = 1
            Const cstQS = 2
            Dim intPARSE_STATE = cstSTART
            Dim intLEN = Len(ur_line) - 1
            Dim stpFIELD As New Strap
            For intIX = 0 To intLEN
                Dim objCHR = objARRAY(intIX)
                Select Case intPARSE_STATE
                    Case cstESC
                        stpFIELD.d(objCHR)
                        intPARSE_STATE = cstQS

                    Case cstQS
                        If objCHR = objESCAPE_QUOTE Then
                            intPARSE_STATE = cstSTART
                            Dim intNEXT_IX = intIX + 1
                            If intNEXT_IX < intLEN Then
                                If objARRAY(intNEXT_IX) = objESCAPE_QUOTE Then
                                    intPARSE_STATE = cstESC
                                End If 'intNEXT_IX
                            End If 'intNEXT_IX

                        Else 'objCHR
                            stpFIELD.d(objCHR)
                        End If 'objCHR

                    Case cstSTART
                        If objCHR = objSPRTR Then
                            sdaCMP.d(stpFIELD.ToString)
                            Call stpFIELD.Clear()

                        ElseIf objCHR = objESCAPE_QUOTE Then
                            intPARSE_STATE = cstQS

                        Else 'objCHR
                            stpFIELD.d(objCHR)
                        End If 'objCHR
                End Select 'intPARSE_STATE
            Next intIX

            sdaCMP.d(stpFIELD)
        End Function 'CSVSplit
    End Class 'MxText-Csv

    Public Class MxText
        Public Class FileName
            Public FilePath As String
            <System.Diagnostics.DebuggerHidden()> Public Sub New()
                Me.FilePath = mt
            End Sub 'New
            <System.Diagnostics.DebuggerHidden()> Public Sub New(ur_path As String)
                Me.FilePath = ur_path
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()> Public Shared Widening Operator CType(b As FileName) As String
                Return b.FilePath
            End Operator 'CType
            <System.Diagnostics.DebuggerHidden()> Public Shared Widening Operator CType(b As String) As FileName
                Return New FileName(b)
            End Operator 'CType
            <System.Diagnostics.DebuggerHidden()> Public Shadows Function ToString() As String
                ToString = Me.FilePath
            End Function 'ToString

            <System.Diagnostics.DebuggerHidden()> Public Function dAppendEXT(ur_ext As String) As FileName
                dAppendEXT = Me
                If ur_ext.StartsWith(".") = False Then
                    ur_ext = "." & ur_ext
                End If

                Me.FilePath &= ur_ext
            End Function 'dAppendEXT

            <System.Diagnostics.DebuggerHidden()> Public Function d(ur_file_name As String) As FileName
                d = Me
                If HasText(Me.FilePath) Then
                    Me.FilePath = System.IO.Path.Combine(Me.FilePath, ur_file_name)

                Else 'Me
                    Me.FilePath = ur_file_name
                End If 'Me
            End Function 'd

            <System.Diagnostics.DebuggerHidden()> Public Function dNowTextYMDHMS(ur_file_name As String, Optional ur_separator As String = "_") As FileName
                dNowTextYMDHMS = Me
                Me.d(ur_file_name & ur_separator & Now().ToString("yyyy\mMM\dddthh\mmm\sss").Replace("P12", "N12").Replace("A12", "A00").ToLower)
            End Function 'dNowTextYMDHMS

            Public Property Ext() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    Ext = System.IO.Path.GetExtension(Me.FilePath)
                End Get

                <System.Diagnostics.DebuggerHidden()> Set(value As String)
                    Me.wEXT(value)
                End Set
            End Property 'Ext

            Public ReadOnly Property ExtAll() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    ExtAll = Strapd().dList(mt, Me.ExtList.ToArray)
                End Get
            End Property 'ExtAll

            <System.Diagnostics.DebuggerHidden()> Public Function ExtList() As Sdata
                Dim sdaDESC = Sdata.Split(Me.FilePath, "."c)
                ExtList = sdaDESC
                sdaDESC.RemoveAt(0)
                For Each ROWCTR In sdaDESC.kvp
                    sdaDESC.Item(ROWCTR) = "." & sdaDESC.Item(ROWCTR)
                Next ROWCTR
            End Function 'ExtList

            Public Property FileGroup() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    FileGroup = System.IO.Path.GetFileNameWithoutExtension(Me.FilePath)
                End Get

                <System.Diagnostics.DebuggerHidden()> Set(value As String)
                    Me.Name = value & Me.Ext
                End Set
            End Property 'FileGroup

            Public Property FileBaseGroup() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    FileBaseGroup = mt
                    For Each strENTRY In Sdata.Split(Me.Name, "."c)
                        FileBaseGroup = strENTRY
                        Exit For
                    Next
                End Get

                <System.Diagnostics.DebuggerHidden()> Set(value As String)
                    Me.Name = value & Me.ExtAll
                End Set
            End Property 'FileBaseGroup

            <System.Diagnostics.DebuggerHidden()> Public Function gCopy() As FileName
                gCopy = New FileName(Me)
            End Function 'gCopy

            <System.Diagnostics.DebuggerHidden()> Public Function gParentDir() As FileName
                gParentDir = New FileName(Me.ParentDir)
            End Function 'gParentDir

            Public Property Name() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    Name = mt
                    Dim strPATH = Me.FilePath
                    If HasText(strPATH) Then
                        If HasText(System.IO.Path.GetFileName(strPATH)) = False Then
                            strPATH = System.IO.Path.GetDirectoryName(strPATH)
                        End If 'strPATH
                    End If

                    If HasText(strPATH) Then
                        Name = System.IO.Path.GetFileName(strPATH)
                    End If
                End Get

                <System.Diagnostics.DebuggerHidden()> Set(value As String)
                    Me.wParentDir.d(value)
                End Set
            End Property 'Name

            Public Property ParentDir() As String
                <System.Diagnostics.DebuggerHidden()> Get
                    ParentDir = mt
                    Dim strPATH = Me.FilePath
                    If HasText(strPATH) Then
                        If HasText(System.IO.Path.GetFileName(strPATH)) = False Then
                            strPATH = System.IO.Path.GetDirectoryName(strPATH)
                        End If 'strPATH
                    End If 'strPATH

                    If HasText(strPATH) Then
                        ParentDir = System.IO.Path.GetDirectoryName(strPATH)
                    End If 'strPATH
                End Get

                <System.Diagnostics.DebuggerHidden()> Set(value As String)
                    Dim strFILE_NAME = Me.Name
                    If HasText(value) Then
                        Me.FilePath = System.IO.Path.Combine(value, strFILE_NAME)

                    Else 'value
                        Me.FilePath = strFILE_NAME
                    End If 'value
                End Set
            End Property 'ParentDir

            <System.Diagnostics.DebuggerHidden()> Public Function ParentList() As System.Collections.Generic.List(Of FileName)
                Dim sdaPATH = Me.PathList
                ParentList = sdaPATH
                sdaPATH.RemoveAt(b0(sdaPATH.Count))
            End Function 'ParentList

            <System.Diagnostics.DebuggerHidden()> Public Function PathList() As System.Collections.Generic.List(Of FileName)
                Dim sdaPATH As New System.Collections.Generic.List(Of FileName)
                PathList = sdaPATH

                Dim sdaDESC = New Objlist(Of FileName)
                sdaDESC.Add(Me.gCopy)
                Dim objREDUCE = Me.gCopy
                Call objREDUCE.wParentDir()
                While HasText(objREDUCE.FilePath)
                    sdaDESC.Add(objREDUCE.gCopy)
                    Call objREDUCE.wParentDir()
                End While 'objREDUCE

                For Each objFILE In sdaDESC
                    sdaPATH.Add(objFILE)
                Next objFILE
            End Function 'PathList

            <System.Diagnostics.DebuggerHidden()> Public Function wAssemblyDir(ur_assembly_info As Microsoft.VisualBasic.ApplicationServices.AssemblyInfo) As FileName
                wAssemblyDir = Me
                Me.FilePath = ur_assembly_info.DirectoryPath.Replace("\bin\Debug", mt)
            End Function 'wAssemblyDir

            <System.Diagnostics.DebuggerHidden()> Public Function wEXT(ur_ext As String) As FileName
                wEXT = Me
                Dim strFILE_NAME = Me.FileGroup
                If Left(ur_ext, 1) <> "." Then
                    ur_ext = "." & ur_ext
                End If 'ur_ext

                Me.wParentDir.d(strFILE_NAME & ur_ext)
            End Function 'wEXT

            <System.Diagnostics.DebuggerHidden()> Public Function wParentDir() As FileName
                wParentDir = Me
                Me.FilePath = Me.ParentDir
            End Function 'wParentDir

            <System.Diagnostics.DebuggerHidden()> Public Function Wrap(ur_path As String) As FileName
                Wrap = Me
                Me.FilePath = mt
                Call Me.d(ur_path)
            End Function 'Wrap

            <System.Diagnostics.DebuggerHidden()> Public Function wTempFileName(ur_fs_proxy As Microsoft.VisualBasic.MyServices.FileSystemProxy) As FileName
                wTempFileName = Me
                Me.FilePath = ur_fs_proxy.GetTempFileName
                Call ur_fs_proxy.DeleteFile(Me.FilePath)
            End Function 'wTempFileName
        End Class 'FileName
    End Class 'MxText-FileName
End Namespace 'Mx
