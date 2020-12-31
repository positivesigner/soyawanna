Option Strict On
Namespace Mx
    Public Module ConstVar
        Public Const mt = ""
        Public Const qs = """"
        Public Const qt = "'"
        Public Const s = " "

        <System.Diagnostics.DebuggerHidden()>
        Public Function AreEqual(ur_val As String, ur_cmp As String) As Boolean
            AreEqual = String.Equals(ur_val, ur_cmp, System.StringComparison.CurrentCultureIgnoreCase)
        End Function
        <System.Diagnostics.DebuggerHidden()>
        Public Function b0(ur_val As Integer) As Integer
            b0 = ur_val - 1
        End Function
        <System.Diagnostics.DebuggerHidden()>
        Public Function b1(ur_val As Integer) As Integer
            b1 = ur_val + 1
        End Function
        <System.Diagnostics.DebuggerHidden()>
        Public Function ContainingText(ur_large_text As String, ur_small_text As String) As Boolean
            ContainingText = InStr(ur_large_text, ur_small_text, CompareMethod.Text) > 0
        End Function 'ContainingText
        <System.Diagnostics.DebuggerHidden()>
        Public Function gUTF8_FileEncoding() As System.Text.UTF8Encoding
            gUTF8_FileEncoding = glbl.gUTF8_Encoding
        End Function
        <System.Diagnostics.DebuggerHidden()>
        Public Function EndingWithText(ur_large_text As String, ur_small_text As String) As Boolean
            EndingWithText = AreEqual(Right(ur_large_text, Len(ur_small_text)), ur_small_text)
        End Function 'EndingWithText
        <System.Diagnostics.DebuggerHidden()>
        Public Function HasText(ur_value As String) As Boolean
            HasText = Not String.IsNullOrWhiteSpace(ur_value)
        End Function 'HasText
        <System.Diagnostics.DebuggerHidden()>
        Public Function StartingWithText(ur_large_text As String, ur_small_text As String) As Boolean
            StartingWithText = AreEqual(Left(ur_large_text, Len(ur_small_text)), ur_small_text)
        End Function 'StartingWithText

        <System.Diagnostics.DebuggerHidden()>
        Public Function CmdOutput(ur_exec_path As String, Optional ur_exec_param As String = mt) As String
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

        <System.Diagnostics.DebuggerHidden()>
        Public Function FileNamed() As MxText.FileName
            FileNamed = New MxText.FileName
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function Strapd() As Strap
            Strapd = New Strap
        End Function

        Public Class glbl
            Private Shared objUTF8_ENCODING As System.Text.UTF8Encoding

            <System.Diagnostics.DebuggerHidden()>
            Private Shared Sub Init()
                If objUTF8_ENCODING Is Nothing Then
                    objUTF8_ENCODING = New System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=False, throwOnInvalidBytes:=True)
                End If
            End Sub 'Init

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function gUTF8_Encoding() As System.Text.UTF8Encoding
                Call glbl.Init()
                gUTF8_Encoding = glbl.objUTF8_ENCODING
            End Function
        End Class 'glbl
    End Module 'ConstVar

    Public Class ErrListBase
        Dim pstpNOTICE_MSG As Strap

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            pstpNOTICE_MSG = Strapd()
        End Sub

        <System.Diagnostics.DebuggerHidden()>
        Public Function DoContinue() As Boolean
            DoContinue = (Me.Found = False)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Sub dError_Data(ur_exception As System.Exception, ur_methodbase As System.Reflection.MethodBase, Optional ur_procedure_status As String = mt)
            If pstpNOTICE_MSG.Length > 0 Then
                pstpNOTICE_MSG.dLine().dLine()
            End If

            pstpNOTICE_MSG.dLine(ur_exception.Message)
            pstpNOTICE_MSG.dLine(Strapd().d("Error in @r1.@r2").r1(ur_methodbase.DeclaringType.Name).r2(ur_methodbase.Name).dS("(status: @r1)").r1(ur_procedure_status))
        End Sub 'dError_Data

        <System.Diagnostics.DebuggerHidden()>
        Public Sub dError_Stack(ur_exception As System.Exception)
            Dim hr As Integer = System.Runtime.InteropServices.Marshal.GetHRForException(ur_exception)
            pstpNOTICE_MSG.dLine(ur_exception.GetType.ToString & "(0x" & hr.ToString("X8") & "): " & ur_exception.Message & System.Environment.NewLine & ur_exception.StackTrace & System.Environment.NewLine)
            Dim st = New System.Diagnostics.StackTrace(ur_exception, True)
            For Each objFRAME In st.GetFrames
                If objFRAME.GetFileLineNumber() > 0 Then
                    pstpNOTICE_MSG.dLine("Line:" & objFRAME.GetFileLineNumber() & " Filename: " & System.IO.Path.GetFileName(objFRAME.GetFileName) & System.Environment.NewLine)
                End If
            Next objFRAME
        End Sub 'dError_Stack

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function Found() As Boolean
            Found = (pstpNOTICE_MSG.Length > 0)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function ToString() As String
            ToString = pstpNOTICE_MSG
        End Function 'ToString

        <System.Diagnostics.DebuggerHidden()>
        Public Sub Throw_Error_Data(ur_exception As System.Exception, ur_methodbase As System.Reflection.MethodBase, Optional ur_procedure_status As String = mt)
            Call Me.dError_Data(ur_exception, ur_methodbase, ur_procedure_status)
            Throw New LocalException(mt)
        End Sub 'Throw_Error_Data


        Public Class LocalException
            Inherits System.Exception

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(message As String)
                MyBase.New(message)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(message As String, inner As System.Exception)
                MyBase.New(message, inner)
            End Sub
        End Class 'LocalException
    End Class 'ErrListBase

    Public Class iWrap(Of T)
        Public v As T

        Public Sub New(ur_t As T)
            Me.v = ur_t
        End Sub
    End Class 'iWrap

    Public Class ObaCTR(Of T)
        Inherits RCTR

        Private vsdaLIST As Obalist(Of T)

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(Optional ur_list As Obalist(Of T) = Nothing)
            Call MyBase.New(prv.TotalItems(ur_list))
            Me.vsdaLIST = ur_list
        End Sub

        Public Shadows ReadOnly Property Current() As ObaCTR(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Current = Me
            End Get
        End Property

        Public Shadows ReadOnly Property v() As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                v = Me.vsdaLIST.Item(Me.Indexenm)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function GetEnumerator() As ObaCTR(Of T)
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Private Class prv
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function TotalItems(Optional ur_list As Obalist(Of T) = Nothing) As Integer
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

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_list As Objlist(Of T))
            Call MyBase.New(ur_list.Count)
            Me.vsdaLIST = ur_list
        End Sub

        Public Shadows ReadOnly Property Current() As ObjCTR(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Current = Me
            End Get
        End Property

        Public Shadows ReadOnly Property row() As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                row = Me.vsdaLIST.Item(Me.Indexenm)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function GetEnumerator() As ObjCTR(Of T)
            GetEnumerator = Me
            Call Me.Reset()
        End Function
    End Class 'ObjCTR

    Public Class Obalist(Of T)
        Private vobjLIST() As T

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(Optional ur_list() As T = Nothing)
            Me.vobjLIST = ur_list
        End Sub

        Public ReadOnly Property Count() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                Count = Me.vobjLIST.Length
            End Get
        End Property

        Public Property Item(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                Item = Me.vobjLIST(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                Me.vobjLIST(ur_index) = ur_val
            End Set
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Function kvp() As ObaCTR(Of T)
            kvp = New ObaCTR(Of T)(Me)
        End Function

        Public Property tr_b1(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                tr_b1 = Me.Item(b0(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                Me.Item(b0(ur_index)) = ur_val
            End Set
        End Property

        Public Property tr_enm(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                tr_enm = Me.Item(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                Me.Item(ur_index) = ur_val
            End Set
        End Property
    End Class 'Obalist

    Public Class Objlist(Of T)
        Inherits System.Collections.Generic.List(Of T)

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function kvp() As ObjCTR(Of T)
            kvp = New ObjCTR(Of T)(Me)
        End Function

        Public Property tr_b1(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                tr_b1 = Me.Item(b0(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                Me.Item(b0(ur_index)) = ur_val
            End Set
        End Property

        Public Property tr_enm(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                tr_enm = Me.Item(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                Me.Item(ur_index) = ur_val
            End Set
        End Property
    End Class 'Objlist

    Public Class SDPCTR(Of T As {New})
        Inherits RCTR

        Private vsdpLIST As SdPair(Of T)

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_sdplist As SdPair(Of T))
            Call MyBase.New(ur_sdplist.Count)
            Me.vsdpLIST = ur_sdplist
        End Sub

        Public Shadows ReadOnly Property Current() As SDPCTR(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Current = Me
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function GetEnumerator() As SDPCTR(Of T)
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Public Shadows ReadOnly Property l() As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                l = Me.vsdpLIST.l_enm(Me.Indexenm)
            End Get
        End Property

        Public Shadows ReadOnly Property v() As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                v = Me.vsdpLIST.v_enm(Me.Indexenm)
            End Get
        End Property
    End Class 'SDPCTR

    Public Class SdPair(Of T As {New})
        Inherits Sdata
        Private vobjT As System.Collections.Generic.Dictionary(Of String, T)

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            vobjT = New System.Collections.Generic.Dictionary(Of String, T)
        End Sub

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function d(ur_val As String, ur_link As T) As SdPair(Of T)
            d = Me
            Call Me.Add(ur_val)
            Call vobjT.Add(ur_val, ur_link)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function kvp() As SDPCTR(Of T)
            kvp = New SDPCTR(Of T)(Me)
        End Function

        Public Property l(ur_key As String) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                l = vobjT.Item(ur_key)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                vobjT.Item(ur_key) = ur_val
            End Set
        End Property

        Public Property l_b1(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                l_b1 = vobjT.Item(Me.v_b1(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                vobjT.Item(Me.v_b1(ur_index)) = ur_val
            End Set
        End Property

        Public Property l_enm(ur_index As Integer) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                l_enm = vobjT.Item(Me.v_enm(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As T)
                vobjT.Item(Me.v_enm(ur_index)) = ur_val
            End Set
        End Property
    End Class 'SdPair

    Public Class RCTR
        Private pintMAX_NUM As Integer
        Private pintCUR_NUM As Integer

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Widening Operator CType(ByVal b As RCTR) As Integer
            Return b.Indexb1
        End Operator 'CType

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_max_num As Integer)
            Me.pintMAX_NUM = ur_max_num
        End Sub

        Public Shadows ReadOnly Property Current() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                Current = Me.pintCUR_NUM
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Function GetEnumerator() As RCTR
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Public ReadOnly Property Indexb1() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                Indexb1 = Me.pintCUR_NUM
            End Get
        End Property

        Public ReadOnly Property Indexenm() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                Indexenm = b0(Me.pintCUR_NUM)
            End Get
        End Property

        Public ReadOnly Property LastIndexb1() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                LastIndexb1 = Me.pintMAX_NUM
            End Get
        End Property

        Public ReadOnly Property LastIndexenm() As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                LastIndexenm = b0(Me.pintMAX_NUM)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Function MoveNext() As Boolean
            MoveNext = (Me.pintCUR_NUM < Me.pintMAX_NUM)
            Me.pintCUR_NUM += 1
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function Reset() As RCTR
            Reset = Me
            Me.pintCUR_NUM = 0
        End Function
    End Class 'RCTR

    Public Class SDACTR
        Inherits RCTR

        Private psdaLIST As Sdata

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_sdata As Sdata)
            Call MyBase.New(ur_sdata.Count)
            Me.psdaLIST = ur_sdata
        End Sub

        Public Shadows ReadOnly Property Current() As SDACTR
            <System.Diagnostics.DebuggerHidden()>
            Get
                Current = Me
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function GetEnumerator() As SDACTR
            GetEnumerator = Me
            Call Me.Reset()
        End Function

        Public Shadows ReadOnly Property v() As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                v = Me.psdaLIST.v_enm(Me.Indexenm)
            End Get
        End Property
    End Class 'SDACTR

    Public Class Sdata
        Inherits System.Collections.Generic.List(Of String)

        <System.Diagnostics.DebuggerHidden()>
        Public Function d(ur_val As String) As Sdata
            d = Me
            Call Me.Add(ur_val)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dList(ParamArray ur_val() As String) As Sdata
            dList = Me
            If ur_val IsNot Nothing Then
                For Each strENTRY In ur_val
                    If strENTRY Is Nothing Then
                        strENTRY = mt
                    End If

                    Call Me.Add(strENTRY)
                Next strENTRY
            End If
        End Function 'dList

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function gCopy() As Sdata
            gCopy = New Sdata().dList(Me.ToArray)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function kvp() As SDACTR
            kvp = New SDACTR(Me)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Split(ur_text As String, ur_sprtr As Char) As Sdata
            Dim sdaCMP = New Sdata
            Split = sdaCMP
            For Each strTEXT In ur_text.Split(ur_sprtr)
                sdaCMP.Add(strTEXT)
            Next
        End Function 'Split

        Public Property v_b1(ur_index As Integer) As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                v_b1 = Me.Item(b0(ur_index))
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As String)
                If ur_val Is Nothing Then
                    ur_val = mt
                End If

                Me.Item(b0(ur_index)) = ur_val
            End Set
        End Property

        Public Property v_enm(ur_index As Integer) As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                v_enm = Me.Item(ur_index)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As String)
                If ur_val Is Nothing Then
                    ur_val = mt
                End If

                Me.Item(ur_index) = ur_val
            End Set
        End Property
    End Class 'Sdata

    Public Class Strap
        Private pstbTEXT As System.Text.StringBuilder

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            Me.pstbTEXT = New System.Text.StringBuilder
        End Sub

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Widening Operator CType(b As Strap) As String
            Return b.ToString
        End Operator

        <System.Diagnostics.DebuggerHidden()>
        Public Function Clear() As Strap
            Clear = Me
            Call Me.pstbTEXT.Clear()
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function d(ur_text As String) As Strap
            d = Me.dSprtr(mt, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dCSV_Quoted(ur_index_b1 As Integer, ur_text As String) As Strap
            dCSV_Quoted = Me
            If ur_index_b1 > 1 Then
                Me.d(",")
            End If

            Me.d(qs).d(ur_text.Replace(qs, qs & qs)).d(qs)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dList(ur_sprtr As String, ParamArray ur_text() As String) As Strap
            dList = Me
            If ur_text IsNot Nothing Then
                For Each strENTRY In ur_text
                    Call Me.dSprtr(ur_sprtr, strENTRY)
                Next strENTRY
            End If
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dCSVList(ParamArray ur_text() As String) As Strap
            dCSVList = Me
            If ur_text IsNot Nothing Then
                For ENTCTR = 0 To UBound(ur_text)
                    Me.dCSV_Quoted(ENTCTR + 1, ur_text(ENTCTR))
                Next ENTCTR
            End If
        End Function 'dCSVList

        <System.Diagnostics.DebuggerHidden()>
        Public Function dLine(Optional ur_text As String = "") As Strap
            dLine = Me.dSprtr(vbCrLf, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dS(ur_text As String) As Strap
            dS = Me.dSprtr(s, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dSprtr(ur_sprtr As String, ur_text As String) As Strap
            dSprtr = Me
            Me.pstbTEXT.Append(ur_sprtr).Append(ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function gCopy(ur_text As String) As Strap
            gCopy = New Strap().d(Me)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function HasText() As Boolean
            HasText = (Me.Length > 0)
        End Function

        Public ReadOnly Property Length As Integer
            <System.Diagnostics.DebuggerHidden()>
            Get
                Length = Me.pstbTEXT.Length
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Function r1(ur_text As String) As Strap
            r1 = Me
            Call Me.pstbTEXT.Replace("@r1", ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function r2(ur_text As String) As Strap
            r2 = Me
            Call Me.pstbTEXT.Replace("@r2", ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function rx(ur_index As Integer, ur_text As String) As Strap
            rx = Me
            Call Me.pstbTEXT.Replace("@r" & ur_index, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function ToString() As String
            ToString = Me.pstbTEXT.ToString
        End Function
    End Class 'Strap

    Public Class bitBASE
        Public name As String
        Public seq As Integer
    End Class

    Public Class TRow(Of T As {New, bitBASE})
        Inherits Sdata

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            For Each intVAL In Me.RefColNames
                Me.Add(mt)
            Next
        End Sub 'New

        Public Shared Function Index_TableSplit(ur_table As Objlist(Of TRow(Of T)), ur_key As T) As SdPair(Of Objlist(Of TRow(Of T)))
            Dim sdpRET = New SdPair(Of Objlist(Of TRow(Of T)))
            Index_TableSplit = sdpRET
            Dim tblCHUNK As Objlist(Of TRow(Of T)) = Nothing
            For Each rowCHUNK In ur_table
                Dim strINDEX = rowCHUNK.v(ur_key)
                Dim intFOUND = sdpRET.IndexOf(strINDEX)
                If intFOUND < 0 Then
                    tblCHUNK = New Objlist(Of TRow(Of T))
                    sdpRET.d(strINDEX, tblCHUNK)
                Else
                    tblCHUNK = sdpRET.l_enm(intFOUND)
                End If

                tblCHUNK.Add(rowCHUNK)
            Next rowCHUNK

            Call sdpRET.Sort()
        End Function 'Index_TableSplit


        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function gCopy() As TRow(Of T)
            Dim sdaRET = New TRow(Of T)
            gCopy = sdaRET
            For Each kvpENTRY In Me.kvp
                sdaRET.v_enm(kvpENTRY.Indexenm) = kvpENTRY.v
            Next kvpENTRY
        End Function

        Public Function Link_Table(ur_table As Objlist(Of TRow(Of T)), ParamArray ur_key_list() As T) As Objlist(Of TRow(Of T))
            Dim tblRET = New Objlist(Of TRow(Of T))
            Link_Table = tblRET
            For Each rowCHUNK In ur_table
                Dim bolFOUND = True
                For Each intKEY In ur_key_list
                    If AreEqual(rowCHUNK.v(intKEY), Me.v(intKEY)) = False Then
                        bolFOUND = False
                        Exit For
                    End If
                Next intKEY

                If bolFOUND Then
                    tblRET.Add(rowCHUNK)
                End If
            Next rowCHUNK
        End Function 'Link_Table

        <System.Diagnostics.DebuggerHidden()>
        Public Function RefColKeys() As Objlist(Of T)
            RefColKeys = TRow(Of T).glbl.RefKeys
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function RefColNames() As Sdata
            RefColNames = TRow(Of T).glbl.RefNames
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function ToCbrd(ur_hdr As Boolean) As Integer
            ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Overloads Function ToString(ur_hdr As Boolean) As Strap
            Dim stpRET = New Strap
            ToString = stpRET
            If ur_hdr Then : For Each kvpCOL In Me.RefColNames.kvp : If kvpCOL.Indexb1 > 1 Then : stpRET.d(vbTab) : End If : stpRET.d(kvpCOL.v) : Next kvpCOL : stpRET.dLine() : End If
            For Each kvpCOL In Me.RefColNames.kvp : If kvpCOL.Indexb1 > 1 Then : stpRET.d(vbTab) : End If : stpRET.d(Me.v_enm(kvpCOL.Indexenm)) : Next kvpCOL : stpRET.dLine()
        End Function


        Public Property v(ur_cl As T) As String
            <System.Diagnostics.DebuggerHidden()>
            Get
                v = Me.Item(ur_cl.seq)
            End Get
            <System.Diagnostics.DebuggerHidden()>
            Set(ur_val As String)
                If ur_val Is Nothing Then
                    ur_val = ""
                End If

                Me.Item(ur_cl.seq) = ur_val
            End Set
        End Property

        Public Class glbl
            Private Shared sdaTCOL_NAME As Sdata
            Private Shared sdjTCOL_KEY As Objlist(Of T)

            <System.Diagnostics.DebuggerHidden()>
            Private Shared Sub Init()
                If sdjTCOL_KEY Is Nothing AndAlso
                  sdaTCOL_NAME Is Nothing Then
                    Dim objTEST = New T
                End If

                If sdjTCOL_KEY Is Nothing AndAlso
                  sdaTCOL_NAME IsNot Nothing Then
                    sdjTCOL_KEY = New Objlist(Of T)
                    Dim tpeCMP = GetType(T)
                    For Each kvpCOL In sdaTCOL_NAME.kvp
                        For Each objPROP In tpeCMP.GetFields()
                            If tpeCMP.DeclaringType Is tpeCMP Then
                                Dim objBIT = DirectCast(objPROP.GetValue(Nothing), bitBASE)
                                If objBIT.seq = kvpCOL.Indexenm Then
                                    sdjTCOL_KEY.Add(DirectCast(objPROP.GetValue(Nothing), T))
                                    Exit For
                                End If
                            End If 'tpeCMP
                        Next objPROP
                    Next kvpCOL
                End If 'sdaTCOL_NAME
            End Sub 'Init

            <System.Diagnostics.DebuggerHidden()>
            Private Shared Function NewProp() As T
                Dim objRET = New T
                NewProp = objRET

                If sdaTCOL_NAME Is Nothing Then
                    sdaTCOL_NAME = New Sdata
                    sdjTCOL_KEY = New Objlist(Of T)
                End If

                Dim intNEXT_SEQ = sdaTCOL_NAME.Count
                Dim tpeB = GetType(T)
                For Each objPROP In tpeB.GetFields()
                    If objPROP.DeclaringType Is tpeB Then
                        Dim objFOUND = objPROP.GetValue(Nothing)
                        If objFOUND Is Nothing Then
                            objRET.name = objPROP.Name
                            objRET.seq = intNEXT_SEQ
                            sdaTCOL_NAME.Add(objRET.name)
                            sdjTCOL_KEY.Add(objRET)
                            Exit For
                        ElseIf objFOUND.GetType.DeclaringType Is tpeB AndAlso
                          sdaTCOL_NAME.Contains(objPROP.Name) = False Then
                            objRET.name = objPROP.Name
                            objRET.seq = intNEXT_SEQ
                            sdaTCOL_NAME.Add(objRET.name)
                            sdjTCOL_KEY.Add(objRET)
                            Exit For
                        End If 'objFOUND
                    End If
                Next objPROP
            End Function 'NewProp

            <System.Diagnostics.DebuggerHidden()>
            Private Shared Sub NewProp(ur_prop As T)
                If sdaTCOL_NAME Is Nothing Then
                    sdaTCOL_NAME = New Sdata
                    sdjTCOL_KEY = New Objlist(Of T)
                End If

                Dim intNEXT_SEQ = sdaTCOL_NAME.Count
                Dim tpeB = GetType(T)
                For Each objPROP In tpeB.GetFields()
                    If objPROP.DeclaringType Is tpeB Then
                        Dim objFOUND = objPROP.GetValue(Nothing)
                        If objFOUND Is Nothing Then
                            ur_prop.name = objPROP.Name
                            ur_prop.seq = intNEXT_SEQ
                            sdaTCOL_NAME.Add(ur_prop.name)
                            sdjTCOL_KEY.Add(ur_prop)
                            Exit For
                        ElseIf objFOUND.GetType.DeclaringType Is tpeB AndAlso
                          sdaTCOL_NAME.Contains(objPROP.Name) = False Then
                            ur_prop.name = objPROP.Name
                            ur_prop.seq = intNEXT_SEQ
                            sdaTCOL_NAME.Add(ur_prop.name)
                            sdjTCOL_KEY.Add(ur_prop)
                            Exit For
                        End If 'objFOUND
                    End If
                Next objPROP
            End Sub 'Init

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function NewBitBase() As T
                NewBitBase = glbl.NewProp()
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub NewBitBase(ur_prop As T)
                Call glbl.NewProp(ur_prop)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function RefKeys() As Objlist(Of T)
                Call glbl.Init()
                RefKeys = glbl.sdjTCOL_KEY
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function RefNames() As Sdata
                Call glbl.Init()
                RefNames = glbl.sdaTCOL_NAME
            End Function

            Public Class Trbase(Of W As {New, T})
                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function NewBitBase() As W
                    Dim objRET = New W
                    NewBitBase = objRET
                    Call TRow(Of T).glbl.NewBitBase(objRET)
                End Function
            End Class 'Trbase
        End Class 'glbl
    End Class 'Trow

    Partial Public Class MxText 'Parse
        Public Class enmCMD_RET
            Inherits bitBASE
            Public Shared eq As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared fslash As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared qs As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared qs_end As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared txt As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared unk As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
            Public Shared ws As enmCMD_RET = TRow(Of enmCMD_RET).glbl.NewBitBase()
        End Class

        Public Class Cmdline_UB(Of T As {New, bitBASE}, W As {New, bitBASE})
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function CommandLine_UBParm(ur_ub_key As W, ur_ub_val As W, ur_source_text As String, ParamArray ur_default_field_list() As T) As sCMD_RET
                CommandLine_UBParm = prv.CommandLine_UBParm(ur_ub_key, ur_ub_val, ur_source_text, ur_default_field_list)
            End Function

            Class sCMDLINE
                Inherits Mx.Objlist(Of TRow(Of enmCMD_RET))

                <System.Diagnostics.DebuggerHidden()>
                Public Overloads Function ToString(ur_hdr As Boolean) As String
                    Dim stpRET = Strapd() : For Each kvpREC In Me.kvp : stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr)) : Next kvpREC : ToString = stpRET
                End Function 'ToString
                <System.Diagnostics.DebuggerHidden()>
                Public Function ToCbrd(ur_hdr As Boolean) As Integer
                    ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
                End Function
            End Class 'sCMDLINE

            Class sCMD_RET
                Public ttbUB_PARM As Objlist(Of TRow(Of W))
                Public ttbCMD_PARM As sCMDLINE
            End Class

            Partial Private Class prv
                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function CommandLine_UBParm(ur_ub_key As W, ur_ub_val As W, ur_source_text As String, ParamArray ur_default_field_list() As T) As sCMD_RET
                    Dim objRET = New sCMD_RET
                    CommandLine_UBParm = objRET
                    objRET.ttbUB_PARM = New Objlist(Of TRow(Of W))
                    objRET.ttbCMD_PARM = New sCMDLINE
                    Dim rowCMD_PARM = New TRow(Of enmCMD_RET)
                    objRET.ttbCMD_PARM.Add(rowCMD_PARM)
                    Dim sdaDEFAULT_PARMNAME = New Sdata
                    Dim sdaDEST_PARMS = TRow(Of T).glbl.RefNames
                    For Each intUN In ur_default_field_list
                        sdaDEFAULT_PARMNAME.d(intUN.name)
                    Next

                    Dim stpCOMPILE = Strapd()
                    Dim intPREV_CHUNK = enmCMD_RET.ws
                    Dim intCHUNK_C = enmCMD_RET.ws
                    For CHRCTR = 0 To ur_source_text.Length - 1
                        Dim chrC = ur_source_text(CHRCTR)
                        intCHUNK_C = gChunkType(chrC)
                        Dim intSPAN = 1
                        For SPNCTR = CHRCTR + 1 To ur_source_text.Length - 1
                            Dim chrS = ur_source_text(SPNCTR)
                            Dim intSPAN_C = gChunkType(chrS)
                            If intSPAN_C IsNot intCHUNK_C OrElse
                              intSPAN_C Is enmCMD_RET.qs Then
                                'Combine all chunks except for escaped quotes
                                intSPAN = SPNCTR - CHRCTR
                                Exit For
                            End If

                            If SPNCTR = ur_source_text.Length - 1 Then
                                intSPAN = SPNCTR + 1 - CHRCTR
                                Exit For
                            End If
                        Next SPNCTR

                        Dim strCHUNK = ur_source_text.Substring(CHRCTR, intSPAN)
                        If intPREV_CHUNK Is enmCMD_RET.ws Then
                            'Have ws, skip compiled ws; wait for fslash or default parm
                            If intCHUNK_C Is enmCMD_RET.fslash Then
                                'skip compiled fslash
                                intPREV_CHUNK = intCHUNK_C

                            ElseIf intCHUNK_C Is enmCMD_RET.qs Then
                                'skip qs around quoted default parameter
                                rowCMD_PARM.v(enmCMD_RET.fslash) = mt
                                If sdaDEFAULT_PARMNAME.Count > 0 Then
                                    rowCMD_PARM.v(enmCMD_RET.fslash) = sdaDEFAULT_PARMNAME.v_b1(1)
                                    sdaDEFAULT_PARMNAME.RemoveAt(b0(1))
                                End If

                                intPREV_CHUNK = intCHUNK_C

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'skip ws between parameters

                            ElseIf intCHUNK_C Is enmCMD_RET.eq OrElse
                              intCHUNK_C Is enmCMD_RET.txt Then
                                'compile unquoted default parameter
                                rowCMD_PARM.v(enmCMD_RET.fslash) = mt
                                If sdaDEFAULT_PARMNAME.Count > 0 Then
                                    rowCMD_PARM.v(enmCMD_RET.fslash) = sdaDEFAULT_PARMNAME.v_b1(1)
                                    sdaDEFAULT_PARMNAME.RemoveAt(b0(1))
                                End If

                                intPREV_CHUNK = enmCMD_RET.txt
                                stpCOMPILE.Clear.d(strCHUNK)
                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.fslash Then
                            'Have ws-fslash, compile txt parm name; wait for eq
                            If intCHUNK_C Is enmCMD_RET.fslash Then
                                stpCOMPILE.d(strCHUNK)

                            ElseIf intCHUNK_C Is enmCMD_RET.qs Then
                                'qs between parm name and eq is unk
                                intPREV_CHUNK = enmCMD_RET.unk

                            ElseIf intCHUNK_C Is enmCMD_RET.ws OrElse
                              intCHUNK_C Is enmCMD_RET.eq Then
                                'skip compile qe
                                rowCMD_PARM.v(enmCMD_RET.fslash) = stpCOMPILE.ToString
                                stpCOMPILE.Clear()
                                intPREV_CHUNK = enmCMD_RET.eq

                            ElseIf intCHUNK_C Is enmCMD_RET.txt Then
                                'compile txt into parm name
                                stpCOMPILE.d(strCHUNK)
                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.eq Then
                            'Have ws-fslash-eq, skip compile; wait for txt
                            If intCHUNK_C Is enmCMD_RET.fslash Then
                                'flush parameter as "=true"; start new parameter
                                stpCOMPILE.Clear.d("true")
                                Dim objFLUSH = prv.Flush_Parameter(ur_ub_key, ur_ub_val, rowCMD_PARM, stpCOMPILE, intCHUNK_C, sdaDEST_PARMS, objRET)
                                rowCMD_PARM = objFLUSH.rowCMD_PARM
                                intPREV_CHUNK = objFLUSH.intPREV_CHUNK

                            ElseIf intCHUNK_C Is enmCMD_RET.qs Then
                                'skip qs around quoted parameter value
                                intPREV_CHUNK = intCHUNK_C

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'keep ws in eq

                            ElseIf intCHUNK_C Is enmCMD_RET.eq Then
                                'skip eq between paramater name and value

                            ElseIf intCHUNK_C Is enmCMD_RET.txt Then
                                'store param name, compile unquoted parameter value
                                stpCOMPILE.d(strCHUNK)
                                intPREV_CHUNK = intCHUNK_C
                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.txt Then
                            'Have ws-fslash-eq-txt, compile parm val; wait for ws
                            If intCHUNK_C Is enmCMD_RET.fslash OrElse
                              intCHUNK_C Is enmCMD_RET.qs Then
                                'Keep fslash and qs in unquoted param value
                                stpCOMPILE.d(strCHUNK)

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'flush parameter; start new parameter
                                Dim objFLUSH = prv.Flush_Parameter(ur_ub_key, ur_ub_val, rowCMD_PARM, stpCOMPILE, intCHUNK_C, sdaDEST_PARMS, objRET)
                                rowCMD_PARM = objFLUSH.rowCMD_PARM
                                intPREV_CHUNK = objFLUSH.intPREV_CHUNK

                            ElseIf intCHUNK_C Is enmCMD_RET.eq Then
                                'Keep eq in unquoted param value
                                stpCOMPILE.d(strCHUNK)

                            ElseIf intCHUNK_C Is enmCMD_RET.txt Then
                                stpCOMPILE.d(strCHUNK)
                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.qs Then
                            'Have ws-fslash-eq-qs, compile parm val; wait for qs_end
                            If intCHUNK_C Is enmCMD_RET.fslash Then
                                'Keep fslash in quoted param value
                                stpCOMPILE.d(strCHUNK)

                            ElseIf intCHUNK_C Is enmCMD_RET.qs Then
                                'hold parameter; skip this qs and look for another escaped qs or flush the parameter
                                intPREV_CHUNK = enmCMD_RET.qs_end

                            ElseIf intCHUNK_C Is enmCMD_RET.ws OrElse
                              intCHUNK_C Is enmCMD_RET.eq OrElse
                              intCHUNK_C Is enmCMD_RET.txt Then
                                'Keep ws, eq and txt in quoted param value
                                stpCOMPILE.d(strCHUNK)

                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.qs_end Then
                            'Have ws-fslash-eq-qs-qsend, compile parm val without ending quote; wait for qs or ws
                            If intCHUNK_C Is enmCMD_RET.fslash Then
                                'qs_end then fslash without ws between is unk
                                stpCOMPILE.d(strCHUNK)
                                intPREV_CHUNK = enmCMD_RET.unk

                            ElseIf intCHUNK_C Is enmCMD_RET.qs Then
                                'keep escaped qs
                                stpCOMPILE.d(strCHUNK)
                                intPREV_CHUNK = intCHUNK_C

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'flush parameter; start new parameter
                                Dim objFLUSH = prv.Flush_Parameter(ur_ub_key, ur_ub_val, rowCMD_PARM, stpCOMPILE, intCHUNK_C, sdaDEST_PARMS, objRET)
                                rowCMD_PARM = objFLUSH.rowCMD_PARM
                                intPREV_CHUNK = objFLUSH.intPREV_CHUNK

                            ElseIf intCHUNK_C Is enmCMD_RET.eq OrElse
                              intCHUNK_C Is enmCMD_RET.txt Then
                                'qs_end then ws or txt is unk
                                stpCOMPILE.d(strCHUNK)
                                intPREV_CHUNK = enmCMD_RET.unk

                            End If

                        ElseIf intPREV_CHUNK Is enmCMD_RET.unk Then
                            'Have unk, skip compile; wait for ws
                            If intCHUNK_C Is enmCMD_RET.fslash OrElse
                              intCHUNK_C Is enmCMD_RET.qs Then
                                stpCOMPILE.d(strCHUNK)

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'skip parameter; start new parameter
                                rowCMD_PARM.v(enmCMD_RET.txt) = stpCOMPILE.ToString
                                rowCMD_PARM = New TRow(Of enmCMD_RET)
                                objRET.ttbCMD_PARM.Add(rowCMD_PARM)
                                stpCOMPILE.Clear()
                                intPREV_CHUNK = intCHUNK_C

                            ElseIf intCHUNK_C Is enmCMD_RET.eq OrElse
                              intCHUNK_C Is enmCMD_RET.txt Then
                                stpCOMPILE.d(strCHUNK)
                            End If
                        End If

                        CHRCTR += intSPAN - 1

                        If CHRCTR = ur_source_text.Length - 1 Then
                            'On last entry
                            If intCHUNK_C Is enmCMD_RET.fslash OrElse intCHUNK_C Is enmCMD_RET.eq Then
                                'flush default parameter value
                                stpCOMPILE.Clear.d("true")
                                Call prv.Flush_Parameter(ur_ub_key, ur_ub_val, rowCMD_PARM, stpCOMPILE, intCHUNK_C, sdaDEST_PARMS, objRET)

                            ElseIf intCHUNK_C Is enmCMD_RET.qs OrElse intCHUNK_C Is enmCMD_RET.txt Then
                                'flush parameter
                                Call prv.Flush_Parameter(ur_ub_key, ur_ub_val, rowCMD_PARM, stpCOMPILE, intCHUNK_C, sdaDEST_PARMS, objRET)

                            ElseIf intCHUNK_C Is enmCMD_RET.ws Then
                                'skip trailing ws
                            End If
                        End If
                    Next CHRCTR
                End Function 'CommandLine_Chunk

                Private Class Flush_Ret
                    Public rowCMD_PARM As TRow(Of enmCMD_RET)
                    Public intPREV_CHUNK As enmCMD_RET
                End Class

                <System.Diagnostics.DebuggerHidden()>
                Private Shared Function Flush_Parameter(ur_ub_key As W, ur_ub_val As W, ur_cmdparm_row As TRow(Of enmCMD_RET), ur_parm_val As Strap, ur_new_prevchunk As enmCMD_RET, ur_refname_list As Sdata, ur_objret_arl As sCMD_RET) As Flush_Ret
                    Dim objRET = New Flush_Ret
                    Flush_Parameter = objRET
                    objRET.rowCMD_PARM = New TRow(Of enmCMD_RET)
                    ur_objret_arl.ttbCMD_PARM.Add(objRET.rowCMD_PARM)
                    objRET.intPREV_CHUNK = ur_new_prevchunk
                    'flush parameter
                    ur_cmdparm_row.v(enmCMD_RET.txt) = ur_parm_val
                    Dim intFOUND = ur_refname_list.IndexOf(ur_cmdparm_row.v(enmCMD_RET.fslash).ToLower)
                    If intFOUND >= 0 Then
                        With 1 : Dim sdaROW = New TRow(Of W)
                            sdaROW.v(ur_ub_key) = ur_refname_list.v_enm(intFOUND)
                            sdaROW.v(ur_ub_val) = ur_cmdparm_row.v(enmCMD_RET.txt)
                            ur_objret_arl.ttbUB_PARM.Add(sdaROW)
                        End With

                        'Skip parameter if not found
                    End If

                    ur_parm_val.Clear()
                End Function

                <System.Diagnostics.DebuggerHidden()>
                Private Shared Function gChunkType(ur_char As Char) As enmCMD_RET
                    If ur_char = vbCr OrElse
                  ur_char = vbLf OrElse
                  ur_char = " "c OrElse
                  ur_char = vbTab Then
                        gChunkType = enmCMD_RET.ws

                    ElseIf ur_char = "="c OrElse
                  ur_char = ":"c Then
                        gChunkType = enmCMD_RET.eq

                    ElseIf ur_char = "/"c Then
                        gChunkType = enmCMD_RET.fslash

                    ElseIf ur_char = """"c Then
                        gChunkType = enmCMD_RET.qs

                    Else
                        gChunkType = enmCMD_RET.txt
                    End If
                End Function 'gChunkType
            End Class 'prv
        End Class
    End Class 'MxText-Parse

    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function CSVSplit(ByVal ur_line As String, Optional ur_separator As String = ",") As Sdata
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
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.FilePath = mt
            End Sub 'New
            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_path As String)
                Me.FilePath = ur_path
            End Sub 'New

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Widening Operator CType(b As FileName) As String
                Return b.FilePath
            End Operator 'CType
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Widening Operator CType(b As String) As FileName
                Return New FileName(b)
            End Operator 'CType
            <System.Diagnostics.DebuggerHidden()>
            Public Shadows Function ToString() As String
                ToString = Me.FilePath
            End Function 'ToString

            <System.Diagnostics.DebuggerHidden()>
            Public Function dAppendEXT(ur_ext As String) As FileName
                dAppendEXT = Me
                If ur_ext.StartsWith(".") = False Then
                    ur_ext = "." & ur_ext
                End If

                Me.FilePath &= ur_ext
            End Function 'dAppendEXT

            <System.Diagnostics.DebuggerHidden()>
            Public Function d(ur_file_name As String) As FileName
                d = Me
                If HasText(Me.FilePath) Then
                    Me.FilePath = System.IO.Path.Combine(Me.FilePath, ur_file_name)

                Else 'Me
                    Me.FilePath = ur_file_name
                End If 'Me
            End Function 'd

            <System.Diagnostics.DebuggerHidden()>
            Public Function dNowTextYMDHMS(ur_file_name As String, Optional ur_separator As String = "_") As FileName
                dNowTextYMDHMS = Me
                Me.d(ur_file_name & ur_separator & Now().ToString("yyyy\mMM\dddthh\mmm\sss").Replace("P12", "N12").Replace("A12", "A00").ToLower)
            End Function 'dNowTextYMDHMS

            Public Property Ext() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    Ext = System.IO.Path.GetExtension(Me.FilePath)
                End Get

                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.wEXT(value)
                End Set
            End Property 'Ext

            Public ReadOnly Property ExtAll() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    ExtAll = Strapd().dList(mt, Me.ExtList.ToArray)
                End Get
            End Property 'ExtAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function ExtList() As Sdata
                Dim sdaDESC = Sdata.Split(Me.FilePath, "."c)
                ExtList = sdaDESC
                sdaDESC.RemoveAt(0)
                For Each ROWCTR In sdaDESC.kvp
                    sdaDESC.v_b1(ROWCTR) = "." & sdaDESC.v_b1(ROWCTR)
                Next ROWCTR
            End Function 'ExtList

            Public Property FileGroup() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    FileGroup = System.IO.Path.GetFileNameWithoutExtension(Me.FilePath)
                End Get

                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.Name = value & Me.Ext
                End Set
            End Property 'FileGroup

            Public Property FileBaseGroup() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
                    FileBaseGroup = mt
                    For Each strENTRY In Sdata.Split(Me.Name, "."c)
                        FileBaseGroup = strENTRY
                        Exit For
                    Next
                End Get

                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.Name = value & Me.ExtAll
                End Set
            End Property 'FileBaseGroup

            <System.Diagnostics.DebuggerHidden()>
            Public Function gCopy() As FileName
                gCopy = New FileName(Me)
            End Function 'gCopy

            <System.Diagnostics.DebuggerHidden()>
            Public Function gParentDir() As FileName
                gParentDir = New FileName(Me.ParentDir)
            End Function 'gParentDir

            Public Property Name() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
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

                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Me.wParentDir.d(value)
                End Set
            End Property 'Name

            Public Property ParentDir() As String
                <System.Diagnostics.DebuggerHidden()>
                Get
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

                <System.Diagnostics.DebuggerHidden()>
                Set(value As String)
                    Dim strFILE_NAME = Me.Name
                    If HasText(value) Then
                        Me.FilePath = System.IO.Path.Combine(value, strFILE_NAME)

                    Else 'value
                        Me.FilePath = strFILE_NAME
                    End If 'value
                End Set
            End Property 'ParentDir

            <System.Diagnostics.DebuggerHidden()>
            Public Function ParentList() As System.Collections.Generic.List(Of FileName)
                Dim sdaPATH = Me.PathList
                ParentList = sdaPATH
                sdaPATH.RemoveAt(b0(sdaPATH.Count))
            End Function 'ParentList

            <System.Diagnostics.DebuggerHidden()>
            Public Function PathList() As System.Collections.Generic.List(Of FileName)
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

            <System.Diagnostics.DebuggerHidden()>
            Public Function wAssemblyDir(ur_assembly_info As Microsoft.VisualBasic.ApplicationServices.AssemblyInfo) As FileName
                wAssemblyDir = Me
                Me.FilePath = ur_assembly_info.DirectoryPath.Replace("\bin\Debug", mt)
            End Function 'wAssemblyDir

            <System.Diagnostics.DebuggerHidden()>
            Public Function wEXT(ur_ext As String) As FileName
                wEXT = Me
                Dim strFILE_NAME = Me.FileGroup
                If Left(ur_ext, 1) <> "." Then
                    ur_ext = "." & ur_ext
                End If 'ur_ext

                Me.wParentDir.d(strFILE_NAME & ur_ext)
            End Function 'wEXT

            <System.Diagnostics.DebuggerHidden()>
            Public Function wParentDir() As FileName
                wParentDir = Me
                Me.FilePath = Me.ParentDir
            End Function 'wParentDir

            <System.Diagnostics.DebuggerHidden()>
            Public Function Wrap(ur_path As String) As FileName
                Wrap = Me
                Me.FilePath = mt
                Call Me.d(ur_path)
            End Function 'Wrap

            <System.Diagnostics.DebuggerHidden()>
            Public Function wTempFileName(ur_fs_proxy As Microsoft.VisualBasic.MyServices.FileSystemProxy) As FileName
                wTempFileName = Me
                Me.FilePath = ur_fs_proxy.GetTempFileName
                Call ur_fs_proxy.DeleteFile(Me.FilePath)
            End Function 'wTempFileName
        End Class 'FileName
    End Class 'MxText-FileName

    Partial Public Class glbl
        Public Class gCboard
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Clear()
                Call My.Computer.Clipboard.Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function GetText() As String
                GetText = My.Computer.Clipboard.GetText
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function SetText(ur_text As String) As Integer
                If HasText(ur_text) Then
                    Call My.Computer.Clipboard.SetText(ur_text)
                    SetText = 1 + ur_text.Length - ur_text.Replace(vbLf, mt).Length
                Else
                    Call My.Computer.Clipboard.Clear()
                    SetText = 0
                End If
            End Function 'SetText
        End Class 'gCboard

        Public Class gDiagnostics
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function IsRunningWindow(ur_title As String) As Boolean
                IsRunningWindow = False
                For Each objCUR_PROC In System.Diagnostics.Process.GetProcesses()
                    If objCUR_PROC.MainWindowHandle.ToInt32 <> 0 AndAlso
                      AreEqual(objCUR_PROC.MainWindowTitle, ur_title) Then
                        IsRunningWindow = True
                        Exit For
                    End If
                Next objCUR_PROC
            End Function 'IsRunningWindow

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function Read_CommandLineText(ur_exec_path As String, ur_exec_param As String) As String
                Dim prcCOMMAND_LINE = gDiagnostics.Start_CommandLine_Program(ur_exec_path, ur_exec_param)
                Read_CommandLineText = prcCOMMAND_LINE.StandardOutput.ReadToEnd()
                prcCOMMAND_LINE.WaitForExit()
            End Function 'Read_CommandLineText

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function Start_CommandLine_Program(ur_exec_path As String, ur_exec_param As String) As System.Diagnostics.Process
                Dim retPROCESS As New System.Diagnostics.Process()
                Start_CommandLine_Program = retPROCESS
                With 1 : Dim prcINFO = retPROCESS.StartInfo
                    prcINFO.FileName = ur_exec_path
                    prcINFO.Arguments = ur_exec_param
                    prcINFO.UseShellExecute = False
                    prcINFO.RedirectStandardOutput = True
                    prcINFO.CreateNoWindow = True
                End With 'prcINFO

                retPROCESS.Start()
            End Function 'Start_Procses

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Start_Windows_Program(ur_exec_path As String, ur_exec_param As String)
                Dim retPROCESS As New System.Diagnostics.Process()
                With 1 : Dim prcINFO = retPROCESS.StartInfo
                    prcINFO.Verb = "Open"
                    prcINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                    prcINFO.UseShellExecute = True
                End With 'prcINFO

                retPROCESS.Start()
            End Sub 'Start_Procses
        End Class 'gDiagnostics

        Public Class gInteraction
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub AppActivate(ur_window_title As String)
                Microsoft.VisualBasic.Interaction.AppActivate(ur_window_title)
            End Sub
        End Class 'gInteraction

        Public Class gEnvironment
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function CommandLine() As String
                CommandLine = System.Environment.CommandLine
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function ExpandEnvironmentVariables(ur_path As String) As String
                ExpandEnvironmentVariables = System.Environment.ExpandEnvironmentVariables(ur_path)
            End Function
        End Class 'gEnvironment

        Public Class gMsgBox
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function GetResult(ur_message As String, ur_style As MsgBoxStyle, ur_title As String) As MsgBoxResult
                GetResult = MsgBox(ur_message, ur_style, ur_title)
            End Function
        End Class 'gMsgBox

        Public Class gThread
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Sleep(ur_milliseconds As Integer)
                System.Threading.Thread.Sleep(ur_milliseconds)
            End Sub
        End Class 'gThread

        Public Class gWindowsFS
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Delete(ur_path As String)
                System.IO.File.Delete(ur_path)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function GetFiles(ur_search_folder As String, ur_filespec As String, ur_recurse_option As System.IO.SearchOption) As String()
                GetFiles = System.IO.Directory.GetFiles(ur_search_folder, ur_filespec, ur_recurse_option)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function GetDirectories(ur_search_folder As String, ur_filespec As String, ur_recurse_option As System.IO.SearchOption) As String()
                GetDirectories = System.IO.Directory.GetDirectories(ur_search_folder, ur_filespec, ur_recurse_option)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function HasDir(ur_search_folder As String) As Boolean
                HasDir = System.IO.Directory.Exists(ur_search_folder)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function HasFile(ur_search_Path As String) As Boolean
                HasFile = System.IO.File.Exists(ur_search_Path)
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Move(ur_source_path As String, ur_dest_path As String)
                System.IO.File.Move(ur_source_path, ur_dest_path)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function ReadAllText(ur_file_path As String) As String
                ReadAllText = System.IO.File.ReadAllText(ur_file_path, Mx.gUTF8_FileEncoding())
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub WriteAllText(ur_file_path As String, ur_text As String)
                Call System.IO.File.WriteAllText(ur_file_path, ur_text, Mx.gUTF8_FileEncoding())
            End Sub
        End Class 'gWindowsFS
    End Class 'glbl


    Partial Public Class Have
        Private Shared tblClipboard As sClipboard
        Private Shared tblMessageBox As sMessageBox
        Private Shared tblWindowsFS As sWindowsFS

        <System.Diagnostics.DebuggerHidden()> Private Shared Sub ConnectMxBase()
            If Have.tblClipboard Is Nothing Then
                Have.tblClipboard = New sClipboard
                Have.tblMessageBox = New sMessageBox
                Have.tblWindowsFS = New sWindowsFS
            End If 'sdaTCOL_NAME
        End Sub 'Connect
    End Class 'Have

    Public Class enmCB
        Inherits bitBASE
        Public Shared text As enmCB = TRow(Of enmCB).glbl.NewBitBase()
        Public Shared row_seq As enmCB = TRow(Of enmCB).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function Clipboard() As sClipboard
            Call Have.ConnectMxBase()
            Clipboard = Have.tblClipboard
        End Function

        Public Class rClipboard
            Inherits TRow(Of enmCB)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmCB, ur_val As String) As rClipboard
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rClipboard

        Public Class sClipboard
            Private ttb As Objlist(Of rClipboard)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rClipboard)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub DelAll()
                Me.ttb.Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rClipboard) As rClipboard
                Ins = ur_from
                Me.ttb.Add(ur_from)
                ur_from.v(enmCB.row_seq) = Me.ttb.Count.ToString
                glbl.gCboard.SetText(ur_from.v(enmCB.text))
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFrom_Windows() As rClipboard
                Dim ret = New rClipboard
                InsFrom_Windows = ret
                Me.ttb.Add(
                    ret.
                    vt(enmCB.text, glbl.gCboard.GetText)
                    )
            End Function

            Public ReadOnly Property SelAll() As Objlist(Of rClipboard)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rClipboard
                If Me.ttb.Count = 0 Then
                    SelFirst = New rClipboard()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean) As String
                Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                    stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                Next kvpREC : ToString = stpRET
            End Function 'ToString
            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sClipboard
    End Class 'CB

    Public Class enmMB
        Inherits bitBASE
        Public Shared title As enmMB = TRow(Of enmMB).glbl.NewBitBase()
        Public Shared text As enmMB = TRow(Of enmMB).glbl.NewBitBase()
        Public Shared user_response As enmMB = TRow(Of enmMB).glbl.NewBitBase()
        Public Shared row_seq As enmMB = TRow(Of enmMB).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function MessageBox() As sMessageBox
            Call Have.ConnectMxBase()
            MessageBox = Have.tblMessageBox
        End Function

        Public Class rMessageBox
            Inherits TRow(Of enmMB)
            Public vUserResponse As MsgBoxResult

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmMB, ur_val As String) As rMessageBox
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rMessageBox

        Public Class sMessageBox
            Private ttb As Objlist(Of rMessageBox)
            Private gMsgBox As glbl.gMsgBox

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rMessageBox)
                Me.gMsgBox = New glbl.gMsgBox
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub DelAll()
                Me.ttb.Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function Ins(ur_from As rMessageBox, ur_style As MsgBoxStyle) As rMessageBox
                Ins = ur_from
                Me.ttb.Add(ur_from)
                ur_from.vUserResponse = glbl.gMsgBox.GetResult(ur_from.v(enmMB.text), ur_style, ur_from.v(enmMB.title))
                ur_from.v(enmMB.user_response) = ur_from.vUserResponse.ToString
                ur_from.v(enmMB.row_seq) = Me.ttb.Count.ToString
            End Function

            Public ReadOnly Property SelAll() As Objlist(Of rMessageBox)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rMessageBox
                If Me.ttb.Count = 0 Then
                    SelFirst = New rMessageBox()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean) As String
                Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                    stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                Next kvpREC : ToString = stpRET
            End Function 'ToString
            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sMessageBox
    End Class 'MB

    Public Class enmWF
        Inherits bitBASE
        Public Shared file_path As enmWF = TRow(Of enmWF).glbl.NewBitBase()
    End Class

    Partial Public Class Have
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function WindowsFS() As sWindowsFS
            Call Have.ConnectMxBase()
            WindowsFS = Have.tblWindowsFS
        End Function

        Public Class rWindbowsFS
            Inherits TRow(Of enmWF)

            <System.Diagnostics.DebuggerHidden()>
            Public Function vt(ur_enm As enmWF, ur_val As String) As rWindbowsFS
                vt = Me
                Me.v(ur_enm) = ur_val
            End Function
        End Class 'rWindowsFS

        Public Class sWindowsFS
            Private ttb As Objlist(Of rWindbowsFS)

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New()
                Me.ttb = New Objlist(Of rWindbowsFS)
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Sub DelAll()
                Me.ttb.Clear()
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsDir_From_Windows(ur_search_folder As String) As sWindowsFS
                InsDir_From_Windows = Me
                Me.DelAll()
                If glbl.gWindowsFS.HasDir(ur_search_folder) Then
                    Me.ttb.Add(
                        New rWindbowsFS().
                        vt(enmWF.file_path, ur_search_folder)
                        )
                End If
            End Function 'InsDir_From_Windows

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsFile_From_Windows(ur_search_folder As String) As sWindowsFS
                InsFile_From_Windows = Me
                Me.DelAll()
                If glbl.gWindowsFS.HasFile(ur_search_folder) Then
                    Me.ttb.Add(
                        New rWindbowsFS().
                        vt(enmWF.file_path, ur_search_folder)
                        )
                End If
            End Function 'InsFile_From_Windows

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsDirList_From_Windows(ur_search_folder As String, ur_filespec As String, ur_recurse_option As System.IO.SearchOption) As sWindowsFS
                InsDirList_From_Windows = Me
                Me.DelAll()
                For Each strPATH In glbl.gWindowsFS.GetDirectories(ur_search_folder, ur_filespec, ur_recurse_option)
                    Me.ttb.Add(
                        New rWindbowsFS().
                        vt(enmWF.file_path, strPATH)
                        )
                Next strPATH
            End Function 'InsDirList_From_Windows

            <System.Diagnostics.DebuggerHidden()>
            Public Function InsList_From_Windows(ur_search_folder As String, ur_filespec As String, ur_recurse_option As System.IO.SearchOption) As sWindowsFS
                InsList_From_Windows = Me
                Me.DelAll()
                For Each strPATH In glbl.gWindowsFS.GetFiles(ur_search_folder, ur_filespec, ur_recurse_option)
                    Me.ttb.Add(
                        New rWindbowsFS().
                        vt(enmWF.file_path, strPATH)
                        )
                Next strPATH
            End Function 'InsList_From_Windows

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel(ur_col As enmWF, ur_value As String) As sWindowsFS
                Dim retCART = New sWindowsFS
                Sel = retCART
                For Each rowCART In Me.ttb
                    If AreEqual(rowCART.v(ur_col), ur_value) Then
                        retCART.ttb.Add(rowCART)
                    End If
                Next
            End Function 'Sel

            Public ReadOnly Property SelAll() As Objlist(Of rWindbowsFS)
                <System.Diagnostics.DebuggerHidden()>
                Get
                    SelAll = ttb
                End Get
            End Property 'SelAll

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelFirst() As rWindbowsFS
                If Me.ttb.Count = 0 Then
                    SelFirst = New rWindbowsFS()
                Else
                    SelFirst = Me.ttb.tr_b1(1)
                End If
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Function SelKey(ur_key As enmWF) As rWindbowsFS
                Dim ret As rWindbowsFS = Nothing
                Dim strKEY = ur_key.name
                For Each row In Me.ttb
                    If AreEqual(row.v(enmWF.file_path), strKEY) Then
                        ret = row
                        Exit For
                    End If
                Next row

                If ret Is Nothing Then
                    ret = New rWindbowsFS
                    Me.ttb.Add(
                        ret.
                        vt(enmWF.file_path, ur_key.name)
                        )
                End If

                SelKey = ret
            End Function 'SelKey

            <System.Diagnostics.DebuggerHidden()>
            Public Function Sel_FileList() As Sdata
                Dim retSDATA = New Sdata
                Sel_FileList = retSDATA
                For Each rowCART In Me.ttb
                    retSDATA.d(rowCART.v(enmWF.file_path))
                Next
            End Function 'Sel_FileList

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean) As String
                Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                    stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                Next kvpREC : ToString = stpRET
            End Function 'ToString
            <System.Diagnostics.DebuggerHidden()>
            Public Function ToCbrd(ur_hdr As Boolean) As Integer
                ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
            End Function
        End Class 'sWindbowsFS
    End Class 'WF

    Partial Public Class Have
        Partial Private Class prv_sample
            Private Class Have
                Private Shared tblUserBowl As sUserBowl

                <System.Diagnostics.DebuggerHidden()>
                Private Shared Sub Connect()
                    If Have.tblUserBowl Is Nothing Then
                        Have.tblUserBowl = New sUserBowl
                    End If 'sdaTCOL_NAME
                End Sub 'Connect

                Public Class enmUB
                    Inherits bitBASE
                    Public Shared bowl_name As enmUB = TRow(Of enmUB).glbl.NewBitBase()
                    Public Shared contents As enmUB = TRow(Of enmUB).glbl.NewBitBase()
                End Class

                Public Class enmUN
                    Inherits bitBASE
                    Public Shared app_folder As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared app_name As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared app_path As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared cmdline_audit As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared cmdline_orig As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared cmdline_table As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared extra_p1 As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared extra_p2 As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared in_subfolder As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared path As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                    Public Shared to_clipboard As enmUN = TRow(Of enmUN).glbl.NewBitBase()
                End Class

                <System.Diagnostics.DebuggerHidden()>
                Public Shared Function UserBowl() As sUserBowl
                    Dim bolFIRST_INIT = (Have.tblUserBowl Is Nothing)
                    Call Have.Connect()
                    UserBowl = Have.tblUserBowl
                    If bolFIRST_INIT Then
                        Call Have.tblUserBowl.InsFrom_Application()
                        Have.tblUserBowl.InsKey(enmUN.cmdline_audit, "1")
                        Call Have.tblUserBowl.Cboard_CmdlineAudit()
                    End If
                End Function 'UserBowl

                Public Class rUserBowl
                    Inherits TRow(Of enmUB)

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function vt(ur_enm As enmUB, ur_val As String) As rUserBowl
                        vt = Me
                        Me.v(ur_enm) = ur_val
                    End Function

                    '<System.Diagnostics.DebuggerHidden()>
                    'Public Shadows Function Link_Table(ur_child_table As Have.sReport_Detl, ParamArray ur_key_list() As bitRP) As sReport_Detl
                    '    Dim tblRET = New sReport_Detl
                    '    Link_Table = tblRET
                    '    Dim sdpXLAT = New SdPair(Of bitRT)
                    '    Dim stpKEY_BINDINGS = Strapd()
                    '    For Each keyRP In ur_key_list
                    '        stpKEY_BINDINGS.dS(keyRP.name)
                    '        For Each keyRT In ur_child_table.SelFirst.RefColKeys
                    '            If AreEqual(keyRT.name, keyRP.name) Then
                    '                sdpXLAT.d(keyRP.name, keyRT)
                    '            End If
                    '        Next keyRT
                    '    Next keyRP

                    '    If sdpXLAT.Count <> ur_key_list.Length Then
                    '        Throw New System.Exception("Key bindings not found for RT/RP join: " & stpKEY_BINDINGS.ToString)

                    '    Else
                    '        For Each trwJOIN In ur_child_table.SelAll
                    '            Dim bolFOUND = True
                    '            For Each keyRP In ur_key_list
                    '                Dim keyRT = sdpXLAT.l(keyRP.name)
                    '                If AreEqual(trwJOIN.v(keyRT), Me.v(keyRP)) = False Then
                    '                    bolFOUND = False
                    '                    Exit For
                    '                End If
                    '            Next keyRP

                    '            If bolFOUND Then
                    '                tblRET.Ins(trwJOIN)
                    '            End If
                    '        Next trwJOIN
                    '    End If 'sdpXLAT
                    'End Function 'Link_Table
                End Class 'rUserBowl

                Public Class sUserBowl
                    Private ttb As Objlist(Of rUserBowl)
                    Private PK As Objlist(Of enmUB)

                    <System.Diagnostics.DebuggerHidden()>
                    Public Sub New()
                        Me.ttb = New Objlist(Of rUserBowl)
                        Me.PK = New Objlist(Of enmUB)
                        Me.PK.Add(enmUB.bowl_name)
                    End Sub

                    <System.Diagnostics.DebuggerHidden()>
                    Public Sub Cboard_CmdlineAudit()
                        If HasText(Me.SelKey(enmUN.cmdline_audit).v(enmUB.contents)) Then
                            Dim strAUDIT = Me.ToString(True)
                            Dim ins_msg = Mx.Have.MessageBox.Ins(
                                New Mx.Have.rMessageBox().
                                    vt(enmMB.title, Me.SelKey(enmUN.app_name).v(enmUB.contents)).
                                    vt(enmMB.text, strAUDIT),
                                MsgBoxStyle.OkCancel
                                )
                            If ins_msg.vUserResponse = MsgBoxResult.Ok Then
                                Mx.Have.Clipboard.Ins(
                                    New Mx.Have.rClipboard().
                                    vt(enmCB.text, strAUDIT)
                                    )
                            End If
                        End If
                    End Sub 'Cboard_CmdlineAudit

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function Ins(ur_from As rUserBowl) As rUserBowl
                        Ins = ur_from
                        Dim ttbSEL = Me
                        Dim stpPK_LIST = Strapd()
                        For Each keyPK In Me.PK
                            Dim strCUR_KEY = ur_from.v(keyPK)
                            If stpPK_LIST.Length > 0 Then stpPK_LIST.d(", ")
                            stpPK_LIST.d(strCUR_KEY)
                            If HasText(strCUR_KEY) = False Then
                                Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("PK value must have data:").dS(keyPK.name))
                            Else
                                ttbSEL = ttbSEL.Sel(keyPK, ur_from.v(keyPK))
                            End If
                        Next keyPK

                        If ttbSEL.SelAll.Count > 0 Then
                            Throw New System.Exception(Strapd().d(Me.GetType.Name).dS("must have unique PK values:").dS(stpPK_LIST.ToString))
                        Else
                            Me.ttb.Add(ur_from)
                        End If
                    End Function 'Ins

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function InsKey(ur_key As enmUN, ur_val As String) As rUserBowl
                        Dim ret = Me.SelKey(ur_key)
                        InsKey = ret
                        If HasText(ret.v(enmUB.contents)) Then
                            Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)
                        Else
                            ret.vt(enmUB.contents, ur_val)
                        End If
                    End Function

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function InsFrom_Application() As rUserBowl
                        Dim ret = New rUserBowl
                        InsFrom_Application = ret
                        'Me.InsKey(enmUN.app_name, New MxText.FileName().d(Mx.Class1.SourcePath).FileGroup)
                        'Me.InsKey(enmUN.app_path, Mx.Class1.SourcePath)
                        'Me.InsKey(enmUN.app_folder, Mx.Class1.SourceFolder)

                        Dim arlCMD_RET = MxText.Cmdline_UB(Of enmUN, enmUB).CommandLine_UBParm(enmUB.bowl_name, enmUB.contents, System.Environment.CommandLine)
                        Me.InsKey(enmUN.cmdline_orig, qs & System.Environment.CommandLine.Replace(qs, qs & qs) & qs)
                        Me.InsKey(enmUN.cmdline_table, qs & arlCMD_RET.ttbCMD_PARM.ToString(True).Replace(qs, qs & qs) & qs)
                        For Each rowFOUND In arlCMD_RET.ttbUB_PARM
                            Me.Ins(
                                New Have.rUserBowl().
                                vt(enmUB.bowl_name, rowFOUND.v(enmUB.bowl_name)).
                                vt(enmUB.contents, rowFOUND.v(enmUB.contents))
                                )
                        Next
                    End Function 'InsFrom_Application

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function Sel(ur_col As enmUB, ur_value As String) As sUserBowl
                        Dim retCART = New sUserBowl
                        Sel = retCART
                        For Each rowCART In Me.ttb
                            If AreEqual(rowCART.v(ur_col), ur_value) Then
                                retCART.Ins(rowCART)
                            End If
                        Next
                    End Function 'Sel

                    Public ReadOnly Property SelAll() As Objlist(Of rUserBowl)
                        <System.Diagnostics.DebuggerHidden()>
                        Get
                            SelAll = ttb
                        End Get
                    End Property 'SelAll

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function SelFirst() As rUserBowl
                        If Me.ttb.Count = 0 Then
                            SelFirst = New rUserBowl()
                        Else
                            SelFirst = Me.ttb.tr_b1(1)
                        End If
                    End Function

                    <System.Diagnostics.DebuggerHidden()>
                    Public Function SelKey(ur_key As enmUN) As rUserBowl
                        Dim ret As rUserBowl = Nothing
                        Dim strKEY = ur_key.name
                        For Each row In Me.ttb
                            If AreEqual(row.v(enmUB.bowl_name), strKEY) Then
                                ret = row
                                Exit For
                            End If
                        Next row

                        If ret Is Nothing Then
                            ret = New rUserBowl
                            Me.ttb.Add(
                                ret.
                                vt(enmUB.bowl_name, ur_key.name)
                                )
                        End If

                        SelKey = ret
                    End Function 'SelKey

                    <System.Diagnostics.DebuggerHidden()>
                    Public Overloads Function ToString(ur_hdr As Boolean) As String
                        Dim stpRET = Strapd() : For Each kvpREC In Me.ttb.kvp
                            stpRET.d(kvpREC.row.ToString((kvpREC.Indexb1 = 1) And ur_hdr))
                        Next kvpREC : ToString = stpRET
                    End Function 'ToString
                    <System.Diagnostics.DebuggerHidden()>
                    Public Function ToCbrd(ur_hdr As Boolean) As Integer
                        ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
                    End Function
                End Class 'sUserBowl
            End Class 'Have
        End Class 'prv_sample
    End Class 'Have.prv_sample
End Namespace 'Mx
