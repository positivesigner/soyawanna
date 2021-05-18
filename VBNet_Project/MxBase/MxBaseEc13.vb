Option Strict On
Namespace Mx '2020m05d09 from 2020m04d29
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

            pstpNOTICE_MSG.dLineNB(ur_exception.Message)
            pstpNOTICE_MSG.dLineNB(Strapd().d("Error in @r1.@r2").r1(ur_methodbase.DeclaringType.Name).r2(ur_methodbase.Name).dS("(status: @r1)").r1(ur_procedure_status))
        End Sub 'dError_Data

        <System.Diagnostics.DebuggerHidden()>
        Public Sub dError_Stack(ur_exception As System.Exception)
            Dim hr As Integer = System.Runtime.InteropServices.Marshal.GetHRForException(ur_exception)
            pstpNOTICE_MSG.dLineNB(ur_exception.GetType.ToString & "(0x" & hr.ToString("X8") & "): " & ur_exception.Message & System.Environment.NewLine & ur_exception.StackTrace & System.Environment.NewLine)
            Dim st = New System.Diagnostics.StackTrace(ur_exception, True)
            For Each objFRAME In st.GetFrames
                If objFRAME.GetFileLineNumber() > 0 Then
                    pstpNOTICE_MSG.dLineNB("Line:" & objFRAME.GetFileLineNumber() & " Filename: " & System.IO.Path.GetFileName(objFRAME.GetFileName) & System.Environment.NewLine)
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

        Public Shadows ReadOnly Property SelTop1() As ObjCTR(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Dim lstTEMP = New Objlist(Of T)
                If Me.Indexb1 <= Me.LastIndexb1 Then
                    lstTEMP.Add(Me.row)
                End If

                SelTop1 = New ObjCTR(Of T)(lstTEMP)
            End Get
        End Property 'SelTop1

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

        <System.Diagnostics.DebuggerHidden()>
        Public Shadows Function SelTop1() As Objlist(Of T)
            Dim retLIST = New Objlist(Of T)
            SelTop1 = retLIST
            If Me.Count > 0 Then
                retLIST.Add(Me.tr_b1(1))
            End If
        End Function 'SelTop1

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
        Public Shadows Function SelTop1() As Sdata
            Dim retLIST = New Sdata
            SelTop1 = retLIST
            If Me.Count > 0 Then
                retLIST.Add(Me.v_b1(1))
            End If
        End Function 'SelTop1

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
        Public Function dLineNB(Optional ur_text As String = "") As Strap
            dLineNB = Me.dSprtrNB(vbCrLf, ur_text)
        End Function 'dLineNB

        <System.Diagnostics.DebuggerHidden()>
        Public Function dLine(Optional ur_text As String = "") As Strap
            dLine = Me.dSprtr(vbCrLf, ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dS(Optional ur_text As String = "") As Strap
            dS = Me
            Me.pstbTEXT.Append(s).Append(ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dSNB(Optional ur_text As String = "") As Strap
            dSNB = Me
            If Me.HasText Then
                Me.pstbTEXT.Append(s)
            End If

            Me.pstbTEXT.Append(ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dSprtr(ur_sprtr As String, ur_text As String) As Strap
            dSprtr = Me
            Me.pstbTEXT.Append(ur_sprtr).Append(ur_text)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function dSprtrNB(ur_sprtr As String, ur_text As String) As Strap
            dSprtrNB = Me
            If Me.HasText Then
                Me.pstbTEXT.Append(ur_sprtr)
            End If

            Me.pstbTEXT.Append(ur_text)
        End Function 'dSprtrNB

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

        <System.Diagnostics.DebuggerHidden()>
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
        Public Shared Function PersistRead(ur_persist_path As String) As Objlist(Of TRow(Of T))
            Dim ttbRET = New Objlist(Of TRow(Of T))
            PersistRead = ttbRET
            If Mx.glbl.gWindowsFS.HasFile(ur_persist_path) Then
                Dim bolFIRST_LINE = True
                Dim lstREF_COL = New Objlist(Of T)
                Using stmIN_FILE = Mx.glbl.gWindowsFS.ReadStream(ur_persist_path)
                    Dim strLINE = mt
                    While stmIN_FILE.EndOfStream = False
                        strLINE &= stmIN_FILE.ReadLine
                        Dim bolBALANCED_QUOTES = True
                        For CHRCTR = 0 To strLINE.Length - 1
                            If strLINE(CHRCTR) = """"c Then
                                bolBALANCED_QUOTES = Not bolBALANCED_QUOTES
                            End If
                        Next CHRCTR

                        If bolBALANCED_QUOTES = False Then
                            strLINE &= vbCrLf

                        Else
                            Dim new_row = New TRow(Of T)
                            If Left(strLINE, 1) = vbLf Then
                                strLINE = Mid(strLINE, 2)
                            End If

                            If bolFIRST_LINE = True Then
                                bolFIRST_LINE = False
                                Dim bolCOL_MATCH = False
                                Dim sdaFIRST_ROW = MxText.CSVSplit(strLINE, vbTab)
                                For Each strENTRY In MxText.CSVSplit(strLINE, vbTab)
                                    Dim enmFOUND_KEY As T = Nothing
                                    For Each enmKEY In new_row.RefColKeys
                                        If AreEqual(enmKEY.name, strENTRY) Then
                                            enmFOUND_KEY = enmKEY
                                            bolCOL_MATCH = True
                                            Exit For
                                        End If
                                    Next enmKEY

                                    lstREF_COL.Add(enmFOUND_KEY)
                                Next strENTRY

                                If bolCOL_MATCH = False Then
                                    Exit While
                                End If

                            ElseIf HasText(strLINE) Then
                                For Each kvpCOL In MxText.CSVSplit(strLINE, vbTab).kvp
                                    Dim enmKEY = lstREF_COL.Item(kvpCOL.Indexenm)
                                    If enmKEY IsNot Nothing Then
                                        new_row.v(enmKEY) = kvpCOL.v
                                    End If
                                Next kvpCOL

                                ttbRET.Add(new_row)
                            End If 'bolFIRST_LINE

                            strLINE = mt
                        End If 'bolBALANCED_QUOTES
                    End While
                End Using 'stmIN_FILE
            End If 'ur_persist_path
        End Function 'PersistRead

        <System.Diagnostics.DebuggerHidden()>
        Public Shared Sub PersistWrite(ur_table As Objlist(Of TRow(Of T)), ur_persist_path As MxText.FileName)
            If Mx.glbl.gWindowsFS.HasDir(ur_persist_path.gParentDir) = False Then
                Mx.glbl.gWindowsFS.CreateDirectory(ur_persist_path.gParentDir)
            End If

            Using stmOUT_FILE = Mx.glbl.gWindowsFS.WriteStream(ur_persist_path)
                For Each kvpROW In ur_table.kvp
                    stmOUT_FILE.Write(kvpROW.row.ToString(kvpROW.Indexb1 = 1, True))
                Next
            End Using 'stmOUT_FILE
        End Sub 'PersistWrite

        <System.Diagnostics.DebuggerHidden()>
        Public Function RefColKeys() As Objlist(Of T)
            RefColKeys = TRow(Of T).glbl.RefKeys
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function RefColKeySearch(ur_name As String) As Objlist(Of T)
            RefColKeySearch = TRow(Of T).glbl.RefKeySearch(ur_name)
        End Function 'RefColKeySearch

        <System.Diagnostics.DebuggerHidden()>
        Public Function RefColNames() As Sdata
            RefColNames = TRow(Of T).glbl.RefNames
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function ToCbrd(ur_hdr As Boolean) As Integer
            ToCbrd = Mx.glbl.gCboard.SetText(Me.ToString(ur_hdr))
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As Strap
            Dim stpRET = New Strap
            ToString = stpRET
            If ur_hdr Then : For Each kvpCOL In Me.RefColNames.kvp : If kvpCOL.Indexb1 > 1 Then : stpRET.d(vbTab) : End If : stpRET.d(kvpCOL.v) : Next kvpCOL : stpRET.dLine() : End If
            For Each kvpCOL In Me.RefColNames.kvp : If kvpCOL.Indexb1 > 1 Then : stpRET.d(vbTab) : End If : If ur_quoted Then : stpRET.d(qs).d(Me.v_enm(kvpCOL.Indexenm).Replace(qs, qs & qs)).d(qs) : Else : stpRET.d(Me.v_enm(kvpCOL.Indexenm)) : End If : Next kvpCOL : stpRET.dLine()
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
                    Dim objTEST = GetType(T).GetFields()(0).GetValue(Nothing)
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
            Public Shared Function RefKeySearch(ur_name As String) As Objlist(Of T)
                Call glbl.Init()
                Dim ttbRET = New Objlist(Of T)
                RefKeySearch = ttbRET
                Dim intFOUND_INDEX = TRow(Of T).glbl.RefNames.IndexOf(ur_name)
                If intFOUND_INDEX >= b0(1) Then
                    ttbRET.Add(TRow(Of T).glbl.RefKeys(intFOUND_INDEX))
                End If
            End Function 'RefKeySearch

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

    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function ParseCLine(ur_json_data As String, ur_notice_msg As Strap) As Objlist(Of cline_row_entry)
            ParseCLine = prvCLINE.ParseCline(ur_json_data, ur_notice_msg)
        End Function 'ParseCLine

        Public Class cline_row_entry
            Public key As String
            Public value As String

            <System.Diagnostics.DebuggerHidden()>
            Public Sub New(ur_key As String, ur_value As String)
                Me.key = ur_key
                Me.value = ur_value
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As Strap
                Dim stpRET = New Strap
                ToString = stpRET
                If ur_hdr Then : stpRET.d("key").d(vbTab).d("value").dLine() : End If
                If ur_quoted Then : stpRET.d(qs).d(Me.key.Replace(qs, qs & qs)).d(qs).d(vbTab).d(qs).d(Me.value.Replace(qs, qs & qs)).d(qs).dLine() : Else : stpRET.d(Me.key).d(vbTab).d(Me.value) : End If : stpRET.dLine()
            End Function
        End Class 'cline_row_entry

        Private Class prvCLINE
            Public Class enmCHR_TYPE
                Inherits bitBASE
                Public Shared ws As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared key_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared val_sprtr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_delim As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
                Public Shared string_chr As enmCHR_TYPE = TRow(Of enmCHR_TYPE).glbl.NewBitBase()
            End Class 'enmCHR_TYPE

            Public Class enmCLINE_SCTN
                Inherits bitBASE
                Public Shared ws_prerow As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- space before first key-value pair '- -/key_e1=val_e1
                Public Shared row_ws_preval_sprtr As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon key name and ws char, include ws chars until value open tag "=" '/key_e1- -=val_e1
                Public Shared row_ws_preval As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon value open tag "=" and ws char, include ws chars until value characters '/key_e1=- -val_e1
                '- upon value open tag "=" and ws char, include ws chars until value open qs '/key_e1=- -"val_e1"
                Public Shared row_ws_newkey_sprtr As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon value characters and ws char, include ws chars until key open tag "/" '/key_e1=val_e1- -/key_e2=val_e2
                '- upon value close qs and ws char, include ws chars until key open tag "/" '/key_e1="val_e1"- -/key_e2=val_e2
                Public Shared row_key_no_qs As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- key name cannot be quoted '/-k-ey_e1=val_e1
                Public Shared row_val_in_qs As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon value open qs, include char until value close qs '/key_e1=-"-val_e1"
                Public Shared row_val_in_qs_esc As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon value qs escape character (qs), include next qs character '/key_e1="val_e1-"-"cont_e2"
                '- upon value qs close character (qs), ws char starts a new key name '/key_e1="val_e1-"- /key_e2=val_e2
                Public Shared row_val_no_qs As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- open value non-qs char, include chars until ws char '/key_e1=-2-345
                Public Shared ignore_rem_data As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- error found, stop parsing
                Public Shared ignore_unnamed_val_in_qs As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon quoted value without key name and value separator, include unquoted chars until val close qs '/key_e1=val_e1 -"-val_e2"
                Public Shared ignore_unnamed_val_in_qs_esc As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon quoted qs, include next qs char '/key_e1=val_e1 "val_e2-"-"cont_e2"
                '- upon quoted qs, ws char start a new key name '/key_e1=val_e1 "val_e2-"- /key_e3=val_e3
                Public Shared ignore_unnamed_val_no_qs As enmCLINE_SCTN = TRow(Of enmCLINE_SCTN).glbl.NewBitBase()
                '- upon value text without key name, include chars until next ws char '/key_e1=[-2-345,6789]
            End Class

            Public Class chr_type
                Public group As enmCHR_TYPE
                Public value As String
            End Class

            Public Class kvp_entry
                Public key As Strap
                Public value As Strap

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Me.key = Strapd()
                    Me.value = Strapd()
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Clear()
                    Me.key.Clear()
                    Me.value.Clear()
                End Sub
            End Class 'kvp_entry

            Public Class line_pos
                Public chr_pos As Integer
                Public line_num As Integer
                Public prev_chr As Char

                <System.Diagnostics.DebuggerHidden()>
                Public Sub New()
                    Me.chr_pos = 0
                    Me.line_num = 1
                    Me.prev_chr = s(0)
                End Sub

                <System.Diagnostics.DebuggerHidden()>
                Public Sub Adv(ur_chr As Char)
                    If prev_chr = Chr(10) Then
                        Me.chr_pos = 1
                        Me.line_num += 1

                    Else
                        Me.chr_pos += 1
                    End If 'ur_chr

                    prev_chr = ur_chr
                End Sub 'Adv
            End Class 'line_pos

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function ParseCline(ur_cline_data As String, ur_notice_msg As Strap) As Objlist(Of cline_row_entry)
                Dim retLIST = New Objlist(Of cline_row_entry)
                ParseCline = retLIST
                Dim CHRCTR = 0
                Dim lnpCUR_OFFSET = New line_pos
                Dim sdaIGNORE_STACK = New Sdata
                Dim kvpCUR_COL = New kvp_entry
                Dim chtCUR_ENTRY = New chr_type
                Dim flgCUR_STATE = enmCLINE_SCTN.ws_prerow
                Dim bolLAST_CHAR = False
                Dim intUNNAMED_VALUE_SEQ = b0(1)
                Dim chrCR = Chr(13)
                Dim chrLF = Chr(10)
                Dim chrQS = qs(0)
                Dim chrSP = s(0)
                Dim chrTAB = Chr(9)
                Dim strLIT_UNHANDLED_CHAR_GROUP = "Unhandled char-group"
                Dim strLIT_UNHANDLED_FILE_SECTION = "Unhandled file-section"

                For Each chrENTRY In ur_cline_data
                    chtCUR_ENTRY.value = chrENTRY
                    CHRCTR += 1
                    If CHRCTR = ur_cline_data.Length Then
                        bolLAST_CHAR = True
                    End If

                    lnpCUR_OFFSET.Adv(chrENTRY)
                    If chrENTRY = chrSP OrElse
                      chrENTRY = chrCR OrElse
                      chrENTRY = chrLF OrElse
                      chrENTRY = chrTAB Then 'whitespace
                        chtCUR_ENTRY.group = enmCHR_TYPE.ws
                    ElseIf chrENTRY = "/"c Then 'key_separator
                        chtCUR_ENTRY.group = enmCHR_TYPE.key_sprtr
                    ElseIf chrENTRY = "="c Then 'value_separator
                        chtCUR_ENTRY.group = enmCHR_TYPE.val_sprtr
                    ElseIf chrENTRY = chrQS Then 'string_delimiter
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_delim
                    Else 'string_data_character
                        chtCUR_ENTRY.group = enmCHR_TYPE.string_chr
                    End If

                    If flgCUR_STATE Is enmCLINE_SCTN.ws_prerow Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr Then
                            'key name must follow without whitespace or quotes
                            flgCUR_STATE = enmCLINE_SCTN.row_key_no_qs
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_no_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_ws_preval_sprtr Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr Then
                            'this key must have had a null value, start a new key
                            flgCUR_STATE = enmCLINE_SCTN.row_key_no_qs
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'the quoted or unquoted value must follow after possible whitespace
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_preval

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the whitespace is equivalent to a value separator, so this key has a quoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'the whitespace is equivalent to a value separator, so this key has an unquoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_val_no_qs

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_ws_preval Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr Then
                            'this key must have had a null value, start a new key
                            flgCUR_STATE = enmCLINE_SCTN.row_key_no_qs
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters must be quoted, so this key must have had a null value
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_no_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'this key has a quoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'this key has an unquoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_val_no_qs
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_ws_newkey_sprtr Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'continue

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr Then
                            'key name must follow without whitespace or quotes
                            flgCUR_STATE = enmCLINE_SCTN.row_key_no_qs
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_no_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_key_no_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'key name is followed by value open tag, possibly with whitespace
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_preval_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr Then
                            'this key must have had a null value, start a new key
                            flgCUR_STATE = enmCLINE_SCTN.row_key_no_qs
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'key name is followed by value open tag
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_preval

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'no key separator tag, so skip unnamed value data
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.key.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_val_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters contained in quoted value
                            kvpCUR_COL.value.d(chrENTRY)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            flgCUR_STATE = enmCLINE_SCTN.row_val_in_qs_esc

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_val_in_qs_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'whitespace means the quote was ending the quoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_newkey_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters are not valid escape codes
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            kvpCUR_COL.value.d(chrENTRY)
                            flgCUR_STATE = enmCLINE_SCTN.row_val_in_qs

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'non-structural characters are not valid escape codes
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_val_no_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'whitespace means the non-quoted value ended
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_newkey_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'the non-quoted value must be followed by a space
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            kvpCUR_COL.value.d(chrENTRY)

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_in_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr Then
                            'structural characters contained in quoted value

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim Then
                            'end of quoted value
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_newkey_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_in_qs_esc Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'quoted value ended
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_preval_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            flgCUR_STATE = enmCLINE_SCTN.ignore_unnamed_val_in_qs

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    ElseIf flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_no_qs Then
                        If chtCUR_ENTRY.group Is enmCHR_TYPE.ws Then
                            'the non-quoted value must be followed by a space
                            flgCUR_STATE = enmCLINE_SCTN.row_ws_newkey_sprtr

                        ElseIf chtCUR_ENTRY.group Is enmCHR_TYPE.key_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.val_sprtr OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_delim OrElse
                          chtCUR_ENTRY.group Is enmCHR_TYPE.string_chr Then
                            'continue

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_CHAR_GROUP)
                        End If

                    Else
                        flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                    End If

                    If bolLAST_CHAR Then
                        If flgCUR_STATE Is enmCLINE_SCTN.ws_prerow OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.row_ws_preval_sprtr OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.row_ws_preval OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.row_ws_newkey_sprtr OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.row_key_no_qs Then
                            'assume key has null value

                        ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_val_in_qs Then
                            'the quoted value did not complete by end of data
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET)

                        ElseIf flgCUR_STATE Is enmCLINE_SCTN.row_val_in_qs_esc OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.row_val_no_qs Then
                            Call prvCLINE.AssignKeyValue(retLIST, kvpCUR_COL)

                        ElseIf flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_in_qs OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_in_qs_esc OrElse
                          flgCUR_STATE Is enmCLINE_SCTN.ignore_unnamed_val_no_qs Then
                            'no key name

                        Else
                            flgCUR_STATE = prvCLINE.AssignError(ur_notice_msg, flgCUR_STATE, chtCUR_ENTRY, lnpCUR_OFFSET, strLIT_UNHANDLED_FILE_SECTION)
                        End If
                    End If

                    If flgCUR_STATE Is enmCLINE_SCTN.ignore_rem_data Then
                        Exit For
                    End If
                Next chrENTRY
            End Function 'ParseCLine

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignError(ret_message As Strap, ur_section As enmCLINE_SCTN, ur_chr As chr_type, ur_line_pos As line_pos, Optional ur_error_type As String = "Invalid") As enmCLINE_SCTN
                AssignError = enmCLINE_SCTN.ignore_rem_data
                ret_message.d(ur_error_type).dS(ur_chr.group.name).dS("(").dS(ur_chr.value).dS(")").dS("inside of").dS(ur_section.name).dS("at position:").dS("line").dS(ur_line_pos.line_num.ToString).dS("character").dS(ur_line_pos.chr_pos.ToString)
            End Function 'AssignError

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function AssignKeyValue(ur_list As Objlist(Of cline_row_entry), ur_kvp As kvp_entry) As Boolean
                AssignKeyValue = False
                If HasText(ur_kvp.key) OrElse
                  HasText(ur_kvp.value) Then
                    Dim trwNEW = New cline_row_entry(ur_kvp.key, ur_kvp.value)
                    ur_list.Add(trwNEW)
                End If

                Call ur_kvp.Clear()
            End Function 'AssignKeyValue
        End Class 'prvCLINE
    End Class 'MxText-CLine

    Partial Public Class MxText
        <System.Diagnostics.DebuggerHidden()>
        Public Shared Function CSVSplit(ByVal ur_line As String, Optional ur_separator As String = ",", Optional ur_text_escape As String = """") As Sdata
            Dim sdaCMP = New Sdata
            CSVSplit = sdaCMP
            Dim chrSPRTR = Mid(ur_separator & ",", 1, 1)(0)
            Dim chrQS = Mid(ur_text_escape & """", 1, 1)(0)
            Dim intLEN = ur_line.Length

            Const intGROUP_QS = 0
            Const intGROUP_FIELD_SEPARATOR = 1
            Const intGROUP_STRING_CHAR = 2
            Const intSEC_TEXT = 1
            '- upon field separator (,), include unquoted chars '"val_e1",-v-al_e2
            Const intSEC_OPEN_QUOTE_OR_ESC = 2
            '- upon text escape ("), a second text escape returns a quote character; other goes to quoted text '"val_e1-"-"cont_e1",val_e2
            Const intSEC_QUOTED_TEXT = 3
            '- upon text escape (") and char, include unquoted chars until text escape '-"-val_e1""cont_e1",val_e3
            Const intSEC_CLOSE_QUOTE_OR_ESC = 4
            '- upon text escape (") and char, include unquoted chars until text escape '"val_e1"-"-cont_e1",val_e3
            Dim strLIT_UNHANDLED_CHAR_GROUP = "Unhandled char-group"
            Dim strLIT_UNHANDLED_FILE_SECTION = "Unhandled file-section"

            Dim CHRCTR = 0
            Dim bolLAST_CHAR = False
            Dim stpFIELD As New Strap
            Dim intCHAR_GROUP = intGROUP_STRING_CHAR
            Dim intFILE_SECTION = intSEC_TEXT
            For Each chrENTRY In ur_line
                CHRCTR += 1
                If CHRCTR = intLEN Then
                    bolLAST_CHAR = True
                End If

                If chrENTRY = chrQS Then
                    intCHAR_GROUP = intGROUP_QS
                ElseIf chrENTRY = chrSPRTR Then
                    intCHAR_GROUP = intGROUP_FIELD_SEPARATOR
                Else
                    intCHAR_GROUP = intGROUP_STRING_CHAR
                End If

                If intFILE_SECTION = intSEC_TEXT Then
                    If intCHAR_GROUP = intGROUP_QS Then
                        intFILE_SECTION = intSEC_OPEN_QUOTE_OR_ESC

                    ElseIf intCHAR_GROUP = intGROUP_FIELD_SEPARATOR Then
                        intFILE_SECTION = intSEC_TEXT
                        sdaCMP.d(stpFIELD.ToString)
                        Call stpFIELD.Clear()

                    ElseIf intCHAR_GROUP = intGROUP_STRING_CHAR Then
                        stpFIELD.d(chrENTRY)

                    Else
                        Throw New System.Exception(strLIT_UNHANDLED_CHAR_GROUP)
                    End If

                ElseIf intFILE_SECTION = intSEC_OPEN_QUOTE_OR_ESC Then
                    If intCHAR_GROUP = intGROUP_QS Then
                        intFILE_SECTION = intSEC_TEXT
                        stpFIELD.d(chrENTRY)

                    ElseIf intCHAR_GROUP = intGROUP_FIELD_SEPARATOR OrElse
                      intCHAR_GROUP = intGROUP_STRING_CHAR Then
                        intFILE_SECTION = intSEC_QUOTED_TEXT
                        stpFIELD.d(chrENTRY)

                    Else
                        Throw New System.Exception(strLIT_UNHANDLED_CHAR_GROUP)
                    End If

                ElseIf intFILE_SECTION = intSEC_QUOTED_TEXT Then
                    If intCHAR_GROUP = intGROUP_QS Then
                        intFILE_SECTION = intSEC_CLOSE_QUOTE_OR_ESC

                    ElseIf intCHAR_GROUP = intGROUP_FIELD_SEPARATOR OrElse
                      intCHAR_GROUP = intGROUP_STRING_CHAR Then
                        stpFIELD.d(chrENTRY)

                    Else
                        Throw New System.Exception(strLIT_UNHANDLED_CHAR_GROUP)
                    End If

                ElseIf intFILE_SECTION = intSEC_CLOSE_QUOTE_OR_ESC Then
                    If intCHAR_GROUP = intGROUP_QS Then
                        intFILE_SECTION = intSEC_QUOTED_TEXT
                        stpFIELD.d(chrENTRY)

                    ElseIf intCHAR_GROUP = intGROUP_FIELD_SEPARATOR Then
                        intFILE_SECTION = intSEC_TEXT
                        sdaCMP.d(stpFIELD.ToString)
                        Call stpFIELD.Clear()

                    ElseIf intCHAR_GROUP = intGROUP_STRING_CHAR Then
                        intFILE_SECTION = intSEC_TEXT
                        sdaCMP.d(stpFIELD.ToString)
                        Call stpFIELD.Clear()

                    Else
                        Throw New System.Exception(strLIT_UNHANDLED_CHAR_GROUP)
                    End If

                Else
                    Throw New System.Exception(strLIT_UNHANDLED_FILE_SECTION)
                End If

                If bolLAST_CHAR Then
                    If intFILE_SECTION = intSEC_TEXT OrElse
                      intFILE_SECTION = intSEC_OPEN_QUOTE_OR_ESC OrElse
                      intFILE_SECTION = intSEC_QUOTED_TEXT OrElse
                      intFILE_SECTION = intSEC_CLOSE_QUOTE_OR_ESC Then
                        If stpFIELD.HasText Then
                            sdaCMP.d(stpFIELD.ToString)
                            Call stpFIELD.Clear()
                        End If

                    Else
                        Throw New System.Exception(strLIT_UNHANDLED_FILE_SECTION)
                    End If
                End If

                'If intFILE_SECTION = intIGNORE_REM_DATA Then
                '    Exit For
                'End If
            Next chrENTRY
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
            Public Function gFullPath() As FileName
                gFullPath = New FileName(glbl.gWindowsFS.GetFullPath(Me.FilePath))
            End Function 'gFullPath

            <System.Diagnostics.DebuggerHidden()>
            Public Function gParentDir() As FileName
                gParentDir = New FileName(Me.ParentDir)
            End Function 'gParentDir

            Public ReadOnly Property HasPath() As Boolean
                <System.Diagnostics.DebuggerHidden()>
                Get
                    HasPath = HasText(Me.FilePath)
                End Get
            End Property 'HasPath

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
            Public Function wClear() As FileName
                wClear = Me
                Me.FilePath = mt
            End Function 'wClear

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
                    prcINFO.RedirectStandardError = True
                    prcINFO.CreateNoWindow = True
                End With 'prcINFO

                retPROCESS.Start()
            End Function 'Start_Procses

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Start_Windows_Program(ur_exec_path As String, ur_exec_param As String)
                Dim retPROCESS As New System.Diagnostics.Process()
                With 1 : Dim prcINFO = retPROCESS.StartInfo
                    prcINFO.Verb = "Open"
                    prcINFO.FileName = ur_exec_path
                    prcINFO.Arguments = ur_exec_param
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
            Public Shared Sub Copy(ur_source_path As String, ur_dest_path As String)
                System.IO.File.Copy(ur_source_path, ur_dest_path, True)
            End Sub
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub CreateDirectory(ur_folder_path As String)
                System.IO.Directory.CreateDirectory(ur_folder_path)
            End Sub

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
            Public Shared Function GetFullPath(ur_partial_path As String) As String
                GetFullPath = System.IO.Path.GetFullPath(ur_partial_path)
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
            Public Shared Function ReadStream(ur_file_path As String) As System.IO.StreamReader
                ReadStream = New System.IO.StreamReader(ur_file_path, Mx.gUTF8_FileEncoding())
            End Function

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub WriteAllText(ur_file_path As String, ur_text As String)
                Call System.IO.File.WriteAllText(ur_file_path, ur_text, Mx.gUTF8_FileEncoding())
            End Sub

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function WriteStream(ur_file_path As String) As System.IO.StreamWriter
                WriteStream = New System.IO.StreamWriter(ur_file_path, False, Mx.gUTF8_FileEncoding())
            End Function
        End Class 'gWindowsFS
    End Class 'glbl

    Public Class TablePKEnum(Of E As {New, bitBASE}, P As {New, bitBASE}, T As {New, TRow(Of E)})
        Private ttb As System.Collections.Generic.Dictionary(Of P, T)
        Public KeyList As Objlist(Of P)
        Public PK As E

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(Optional ur_flag_populate_keys As Boolean = True)
            Me.ttb = New System.Collections.Generic.Dictionary(Of P, T)
            Me.PK = TRow(Of E).glbl.RefKeys.tr_b1(1)
            Me.KeyList = TRow(Of P).glbl.RefKeys
            If ur_flag_populate_keys Then
                For Each enmENTRY In Me.KeyList
                    Dim new_row = New T
                    new_row.v(Me.PK) = enmENTRY.name
                    Me.ttb.Add(enmENTRY, new_row)
                Next enmENTRY
            End If 'ur_empty_table
        End Sub 'New

        <System.Diagnostics.DebuggerHidden()>
        Public Function Count() As Integer
            Count = Me.ttb.Count
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function ExistsKey(ur_key As P) As Boolean
            ExistsKey = Me.ttb.ContainsKey(ur_key)
        End Function 'ExistsKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function Ins(ur_row As T) As T
            Ins = ur_row
            Dim enmENTRY = prv.Get_PKStr(Me, ur_row)
            If enmENTRY Is Nothing Then
                Throw New System.Exception("Primary Key must exist in the enumeration: " & ur_row.v(Me.PK))

            ElseIf Me.ttb.ContainsKey(enmENTRY) Then
                Throw New System.Exception("Cannot insert duplicate key for key: " & ur_row.v(Me.PK))

            Else
                Me.ttb.Add(enmENTRY, ur_row)
            End If 'Me
        End Function 'Ins

        <System.Diagnostics.DebuggerHidden()>
        Public Function InsKey(ur_key As P) As T
            Dim ret As T = Nothing
            If Me.ttb.ContainsKey(ur_key) Then
                Throw New System.Exception("Cannot insert duplicate key for key: " & ur_key.name)

            Else
                ret = New T
                Call prv.Assign_PKStr(Me, ret, ur_key)
                Me.ttb.Add(ur_key, ret)
            End If 'Me

            InsKey = ret
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function KeySearch(ur_key_name As String) As Objlist(Of P)
            KeySearch = TRow(Of P).glbl.RefKeySearch(ur_key_name)
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function PersistRead(ur_persist_path As String) As TablePKEnum(Of E, P, T)
            Dim ttbRET = New TablePKEnum(Of E, P, T)
            PersistRead = ttbRET
            For Each row In TRow(Of E).PersistRead(ur_persist_path)
                Dim strPK = row.v(Me.PK)
                For Each enmENTRY In TRow(Of P).glbl.RefKeys
                    If AreEqual(enmENTRY.name, strPK) Then
                        Dim trwCOPY = New T
                        For Each kvpENTRY In trwCOPY.kvp
                            trwCOPY.v_b1(kvpENTRY.Indexb1) = row.v_b1(kvpENTRY.Indexb1)
                        Next

                        Me.ttb.Item(enmENTRY) = trwCOPY
                        Exit For
                    End If
                Next enmENTRY
            Next
        End Function 'PersistRead

        <System.Diagnostics.DebuggerHidden()>
        Public Sub PersistWrite(ur_persist_path As String)
            Dim ttbOUT = New Objlist(Of TRow(Of E))
            For Each entry In Me.ttb.Values
                ttbOUT.Add(entry)
            Next
            Call TRow(Of E).PersistWrite(ttbOUT, ur_persist_path)
        End Sub 'PersistWrite

        <System.Diagnostics.DebuggerHidden()>
        Public Function Sel(ur_col As E, ur_value As String) As TablePKEnum(Of E, P, T)
            Dim retTABLE = New TablePKEnum(Of E, P, T)(False)
            Sel = retTABLE
            For Each kvpROW In Me.ttb
                If AreEqual(kvpROW.Value.v(ur_col), ur_value) Then
                    retTABLE.Ins(kvpROW.Value)
                End If
            Next kvpROW
        End Function 'Sel

        Public ReadOnly Property SelAll() As Objlist(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Dim lstRET = New Objlist(Of T)
                For Each kvpENTRY In Me.ttb
                    lstRET.Add(kvpENTRY.Value)
                Next

                SelAll = lstRET
            End Get
        End Property 'SelAll

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelDistinct(ur_col As E) As Sdata
            Dim ret = New Sdata
            SelDistinct = ret
            For Each kvpROW In Me.ttb

                Dim strVAL = kvpROW.Value.v(ur_col)
                Dim bolNEW_ENTRY = True
                For Each strENTRY In ret
                    If AreEqual(strENTRY, strVAL) Then
                        bolNEW_ENTRY = False
                    End If
                Next strENTRY

                If bolNEW_ENTRY Then
                    ret.Add(strVAL)
                End If
            Next kvpROW

            Call ret.Sort()
        End Function 'SelDistinct

        Public ReadOnly Property SelKey(ur_key As P) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                SelKey = Me.ttb.Item(ur_key)
            End Get
        End Property

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelNe(ur_col As E, ur_value As String) As TablePKEnum(Of E, P, T)
            Dim retTABLE = New TablePKEnum(Of E, P, T)(False)
            SelNe = retTABLE
            For Each kvpROW In Me.ttb
                If AreEqual(kvpROW.Value.v(ur_col), ur_value) = False Then
                    retTABLE.Ins(kvpROW.Value)
                End If
            Next kvpROW
        End Function 'SelNe

        <System.Diagnostics.DebuggerHidden()>
        Public Function UseKey(ur_key As P) As T
            Dim ret As T = Nothing
            If Me.ttb.ContainsKey(ur_key) Then
                ret = Me.ttb.Item(ur_key)

            Else
                ret = New T
                Call prv.Assign_PKStr(Me, ret, ur_key)
                Me.ttb.Add(ur_key, ret)
            End If 'Me

            UseKey = ret
        End Function 'UseKey

        <System.Diagnostics.DebuggerHidden()>
        Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As String
            Dim stpRET = Strapd() : Dim intINDEX = 0 : For Each kvpREC In Me.ttb : intINDEX += 1
                stpRET.d(kvpREC.Value.ToString((intINDEX = 1) And ur_hdr, ur_quoted))
            Next kvpREC : ToString = stpRET
        End Function 'ToString

        Private Class prv
            Public Shared Sub Assign_PKStr(ur_table As TablePKEnum(Of E, P, T), ur_row As T, ur_key As P)
                ur_row.v(ur_table.PK) = ur_key.name
            End Sub 'Assign_PKStr

            Public Shared Function Get_PKStr(ur_table As TablePKEnum(Of E, P, T), ur_row As T) As P
                Dim ret As P = Nothing
                Dim strPK = ur_row.v(ur_table.PK)
                For Each enmENTRY In TRow(Of P).glbl.RefKeys
                    If AreEqual(enmENTRY.name, strPK) Then
                        ret = enmENTRY
                        Exit For
                    End If
                Next enmENTRY

                Get_PKStr = ret
            End Function 'Get_PKStr
        End Class 'prv
    End Class 'TablePKEnum

    Public Class TablePKStr(Of E As {New, bitBASE}, T As {New, TRow(Of E)})
        Private ttb As System.Collections.Generic.Dictionary(Of String, T)
        Public PK As Objlist(Of E)

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New(ur_pk_count As Integer)
            Me.ttb = New System.Collections.Generic.Dictionary(Of String, T)
            Me.PK = New Objlist(Of E)
            With 1 : Dim lstKEYS = TRow(Of E).glbl.RefKeys
                Dim intPK_COUNT = ur_pk_count
                If intPK_COUNT > lstKEYS.Count Then
                    intPK_COUNT = lstKEYS.Count
                End If

                For KEYCTR = 1 To ur_pk_count
                    Me.PK.Add(lstKEYS.Item(b0(KEYCTR)))
                Next
            End With 'lstKEYS
        End Sub 'New

        <System.Diagnostics.DebuggerHidden()>
        Public Function Count() As Integer
            Count = Me.ttb.Count
        End Function

        <System.Diagnostics.DebuggerHidden()>
        Public Function ExistsKey(ParamArray ur_key() As String) As Boolean
            ExistsKey = False
            If ur_key.Length <> Me.PK.Count Then
                Throw New System.Exception("ExistsKey requires all PK parameters: " & Me.PK.Count)

            Else
                Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_key)
                ExistsKey = Me.ttb.ContainsKey(strPK_KEY_COMBINED)
            End If 'ur_key
        End Function 'ExistsKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function DelAll() As TablePKStr(Of E, T)
            DelAll = Me
            Call Me.ttb.Clear()
        End Function 'DelAll

        <System.Diagnostics.DebuggerHidden()>
        Public Function DelKey(ParamArray ur_key() As String) As TablePKStr(Of E, T)
            DelKey = Me
            Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_key)
            Call Me.ttb.Remove(strPK_KEY_COMBINED)
        End Function 'DelKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function Del(ur_row As T) As TablePKStr(Of E, T)
            Del = Me
            Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_row)
            Call Me.ttb.Remove(strPK_KEY_COMBINED)
        End Function 'DelKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function Index_TableSplit(ur_key As E) As SdPair(Of Objlist(Of T))
            Dim sdpRET = New SdPair(Of Objlist(Of T))
            Index_TableSplit = sdpRET
            Dim tblCHUNK As Objlist(Of T) = Nothing
            For Each rowCHUNK In Me.ttb.Values
                Dim strINDEX = rowCHUNK.v(ur_key)
                Dim intFOUND = sdpRET.IndexOf(strINDEX)
                If intFOUND < 0 Then
                    tblCHUNK = New Objlist(Of T)
                    sdpRET.d(strINDEX, tblCHUNK)
                Else
                    tblCHUNK = sdpRET.l_enm(intFOUND)
                End If

                tblCHUNK.Add(rowCHUNK)
            Next rowCHUNK

            Call sdpRET.Sort()
        End Function 'Index_TableSplit

        <System.Diagnostics.DebuggerHidden()>
        Public Function Ins(ur_row As T) As T
            Ins = ur_row
            Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_row)
            If Me.ttb.ContainsKey(strPK_KEY_COMBINED) Then
                Throw New System.Exception("Cannot insert duplicate key for key: " & strPK_KEY_COMBINED)

            Else
                Me.ttb.Add(strPK_KEY_COMBINED, ur_row)
            End If 'Me
        End Function 'Ins

        <System.Diagnostics.DebuggerHidden()>
        Public Function InsKey(ParamArray ur_key() As String) As T
            Dim ret As T = Nothing
            If ur_key.Length <> Me.PK.Count Then
                Throw New System.Exception("InsKey requires all PK parameters: " & Me.PK.Count)

            Else
                Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_key)
                If Me.ttb.ContainsKey(strPK_KEY_COMBINED) Then
                    Throw New System.Exception("Cannot insert duplicate key for key: " & strPK_KEY_COMBINED)

                Else
                    ret = New T
                    Call prv.Assign_PKStr(Me, ret, ur_key)
                    Me.ttb.Add(strPK_KEY_COMBINED, ret)
                End If 'Me
            End If 'ur_key

            InsKey = ret
        End Function 'InsKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function PersistRead(ur_persist_path As String) As TablePKStr(Of E, T)
            PersistRead = Me
            For Each row In TRow(Of E).PersistRead(ur_persist_path)
                Dim new_row = New T
                For Each enmENTRY In new_row.RefColKeys
                    new_row.v(enmENTRY) = row.v(enmENTRY)
                Next

                Me.Ins(new_row)
            Next
        End Function 'PersistRead

        <System.Diagnostics.DebuggerHidden()>
        Public Sub PersistWrite(ur_persist_path As String)
            Dim ttbOUT = New Objlist(Of TRow(Of E))
            For Each entry In Me.ttb.Values
                ttbOUT.Add(entry)
            Next
            Call TRow(Of E).PersistWrite(ttbOUT, ur_persist_path)
        End Sub 'PersistWrite

        <System.Diagnostics.DebuggerHidden()>
        Public Function Sel(ur_col As E, ur_value As String) As TablePKStr(Of E, T)
            Dim retTABLE = New TablePKStr(Of E, T)(Me.PK.Count)
            Sel = retTABLE
            For Each kvpROW In Me.ttb
                If AreEqual(kvpROW.Value.v(ur_col), ur_value) Then
                    retTABLE.Ins(kvpROW.Value)
                End If
            Next kvpROW
        End Function 'Sel

        Public ReadOnly Property SelAll() As Objlist(Of T)
            <System.Diagnostics.DebuggerHidden()>
            Get
                Dim lstRET = New Objlist(Of T)
                For Each kvpENTRY In Me.ttb
                    lstRET.Add(kvpENTRY.Value)
                Next

                SelAll = lstRET
            End Get
        End Property 'SelAll

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelDistinct(ur_col As E) As Sdata
            Dim ret = New Sdata
            SelDistinct = ret
            For Each kvpROW In Me.ttb

                Dim strVAL = kvpROW.Value.v(ur_col)
                Dim bolNEW_ENTRY = True
                For Each strENTRY In ret
                    If AreEqual(strENTRY, strVAL) Then
                        bolNEW_ENTRY = False
                    End If
                Next strENTRY

                If bolNEW_ENTRY Then
                    ret.Add(strVAL)
                End If
            Next kvpROW

            Call ret.Sort()
        End Function 'SelDistinct

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelTop1() As Objlist(Of T)
            Dim lstRET = New Objlist(Of T)
            For Each kvpENTRY In Me.ttb
                lstRET.Add(kvpENTRY.Value)
                Exit For
            Next

            SelTop1 = lstRET
        End Function 'SelTop1

        Public ReadOnly Property SelKey(ParamArray ur_key() As String) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                Dim ret As T = Nothing
                If ur_key.Length <> Me.PK.Count Then
                    Throw New System.Exception("SelKey requires all PK parameters: " & Me.PK.Count)

                Else
                    Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_key)
                    If Me.ttb.ContainsKey(strPK_KEY_COMBINED) Then
                        ret = Me.ttb.Item(strPK_KEY_COMBINED)

                    Else
                        ret = New T
                    End If 'Me
                End If 'ur_key

                SelKey = ret
            End Get
        End Property 'SelKey

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelNe(ur_col As E, ur_value As String) As TablePKStr(Of E, T)
            Dim retTABLE = New TablePKStr(Of E, T)(Me.PK.Count)
            SelNe = retTABLE
            For Each kvpROW In Me.ttb
                If AreEqual(kvpROW.Value.v(ur_col), ur_value) = False Then
                    retTABLE.Ins(kvpROW.Value)
                End If
            Next kvpROW
        End Function 'SelNe

        <System.Diagnostics.DebuggerHidden()>
        Public Function SelOnKey(ParamArray ur_key() As String) As TablePKStr(Of E, T)
            Dim ret = Me
            For KEYCTR = 1 To Me.PK.Count
                If ur_key.Length >= KEYCTR Then
                    ret = ret.Sel(Me.PK.Item(b0(KEYCTR)), ur_key(b0(KEYCTR)))
                End If
            Next KEYCTR

            SelOnKey = ret
        End Function 'SelOnKey

        <System.Diagnostics.DebuggerHidden()>
        Public Overloads Function ToString(ur_hdr As Boolean, Optional ur_quoted As Boolean = True) As String
            Dim stpRET = Strapd() : Dim intINDEX = 0 : For Each kvpREC In Me.ttb : intINDEX += 1
                stpRET.d(kvpREC.Value.ToString((intINDEX = 1) And ur_hdr, ur_quoted))
            Next kvpREC : ToString = stpRET
        End Function 'ToString

        <System.Diagnostics.DebuggerHidden()>
        Public Function UseKey(ParamArray ur_key() As String) As T
            Dim ret As T = Nothing
            Dim strPK_KEY_COMBINED = prv.Get_PKStr(Me, ur_key)
            If Me.ttb.ContainsKey(strPK_KEY_COMBINED) Then
                ret = Me.ttb.Item(strPK_KEY_COMBINED)

            Else
                ret = New T
                Call prv.Assign_PKStr(Me, ret, ur_key)
                Me.ttb.Add(strPK_KEY_COMBINED, ret)
            End If

            UseKey = ret
        End Function 'UseKey

        Private Class prv
            <System.Diagnostics.DebuggerHidden()>
            Public Shared Sub Assign_PKStr(ur_table As TablePKStr(Of E, T), ur_row As T, ur_key() As String)
                Dim intPK_KEYS = ur_key.Length
                If intPK_KEYS > ur_table.PK.Count Then
                    intPK_KEYS = ur_table.PK.Count
                End If

                For KEYCTR = 1 To intPK_KEYS
                    ur_row.v(ur_table.PK.Item(b0(KEYCTR))) = ur_key(b0(KEYCTR))
                Next
            End Sub 'Assign_PKStr

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function Get_PKStr(ur_table As TablePKStr(Of E, T), ur_key() As String) As String
                Dim intPK_KEYS = ur_key.Length
                If intPK_KEYS > ur_table.PK.Count Then
                    intPK_KEYS = ur_table.PK.Count
                End If

                Dim stpPK_KEY_COMBINED = Strapd()
                For KEYCTR = 1 To intPK_KEYS
                    If KEYCTR > 1 Then stpPK_KEY_COMBINED.d(vbTab)
                    stpPK_KEY_COMBINED.d(ur_key(b0(KEYCTR)).ToUpper)
                Next

                Get_PKStr = stpPK_KEY_COMBINED.ToString
            End Function 'Get_PKStr

            <System.Diagnostics.DebuggerHidden()>
            Public Shared Function Get_PKStr(ur_table As TablePKStr(Of E, T), ur_row As T) As String
                Dim stpPK_KEY_COMBINED = Strapd()
                For KEYCTR = 1 To ur_table.PK.Count
                    If KEYCTR > 1 Then stpPK_KEY_COMBINED.d(vbTab)
                    stpPK_KEY_COMBINED.d(ur_row.v_b1(KEYCTR).ToUpper)
                Next

                Get_PKStr = stpPK_KEY_COMBINED.ToString
            End Function 'Get_PKStr
        End Class 'prv
    End Class 'TablePKStr

    Public Class TablePKStrOne(Of E As {New, bitBASE}, P As {New, bitBASE}, T As {New, TRow(Of E)})
        Inherits TablePKStr(Of E, T)
        Public KeyList As Objlist(Of P)

        <System.Diagnostics.DebuggerHidden()>
        Public Sub New()
            Call MyBase.New(1)
            Me.KeyList = TRow(Of P).glbl.RefKeys
        End Sub

        Public Shadows ReadOnly Property SelKey(ur_key As P) As T
            <System.Diagnostics.DebuggerHidden()>
            Get
                Dim retROW As T = Nothing
                For Each trwENTRY In Me.SelOnKey(ur_key.name).SelAll
                    retROW = trwENTRY
                Next

                If retROW Is Nothing Then
                    retROW = Me.InsKey(ur_key.name)
                End If

                SelKey = retROW
            End Get
        End Property 'SelKey
    End Class 'TablePKStrOne
End Namespace 'Mx
