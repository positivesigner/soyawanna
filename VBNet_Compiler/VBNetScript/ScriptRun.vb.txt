Namespace Mx
    Public Class ScriptRun
        ' Build a Hello World program graph using 
        ' System.CodeDom types.
        Public Class enmC_V_J
            Inherits bitBASE
            Public Shared CSharp As enmC_V_J = TRow(Of enmC_V_J).glbl.NewBitBase()
            Public Shared VisualBasic As enmC_V_J = TRow(Of enmC_V_J).glbl.NewBitBase()
            Public Shared JScript As enmC_V_J = TRow(Of enmC_V_J).glbl.NewBitBase()
        End Class 'enmC_V_J

        Public Shared Function RunCode(ur_provider As Mx.ScriptRun.enmC_V_J, ur_input_script As String, ur_assembly_folder As String) As String
            Dim strSOURCE_CODE = mt
            Dim stpERR_MSG = Strapd()
            Dim stpCODE_FILE = Strapd()
            ' Configure a CompilerParameters that links System.dll and produces the specified executable file.
            Dim objCOMPILER_PARM As New System.CodeDom.Compiler.CompilerParameters()
            Dim objCOPMILER_RESULT As System.CodeDom.Compiler.CompilerResults = Nothing
            Dim objMETHOD_MAIN As System.Reflection.MethodInfo = Nothing
            'start "" VBNetScript.exe /path script.vbns
            'exit
            'Optional - UrRef.dll
            'Optional - UrInclude.vb
            'Optional - Option Strict On, or some other Option On/Off
            'Optional - Imports UrNamespace
            'UrCodeLines
            Using stmFILE = New System.IO.StreamReader(ur_input_script, gUTF8_FileEncoding)
                Dim sdaEXTERNAL_FILES As New Objlist(Of MxText.FileName)
                objCOMPILER_PARM.ReferencedAssemblies.Add("system.dll")
                Dim strLINE = mt
                While stmFILE.EndOfStream = False
                    If HasText(strLINE) = False Then
                        strLINE = stmFILE.ReadLine
                    End If

                    If StartingWithText(strLINE.Trim, "start" & s) OrElse
                    StartingWithText(strLINE.Trim, "exit") Then
                        strLINE = mt

                    ElseIf EndingWithText(strLINE.Trim, ".DLL") Then
                        objCOMPILER_PARM.ReferencedAssemblies.Add(strLINE.Trim)
                        strLINE = mt

                    ElseIf EndingWithText(strLINE.Trim, ".VB") Then
                        sdaEXTERNAL_FILES.Add(strLINE.Trim)
                        strLINE = mt

                    ElseIf StartingWithText(strLINE.Trim, "OPTION" & s) OrElse
                    StartingWithText(strLINE.Trim, "IMPORTS" & s) Then
                        stpCODE_FILE.dLine(strLINE)
                        strLINE = mt

                    Else 'strLINE
                        Exit While
                    End If 'strLINE
                End While 'stmFILE

                strSOURCE_CODE = strLINE & Constants.vbCrLf & stmFILE.ReadToEnd
                'cp.ReferencedAssemblies.Add("system.xml.dll")
                'cp.ReferencedAssemblies.Add("system.data.dll")
                'cp.CompilerOptions = "/t:library"
                objCOMPILER_PARM.GenerateInMemory = True
                objCOMPILER_PARM.GenerateExecutable = False

                ' Generate the Code Framework
                'The program automatically adds NameSpace Mx
                'The program automatically adds Class Class1
                'The program automatically adds Function RetVal() As String
                'UrFunctionines
                'Optional - RetVal = UrReturnValue
                'Optional - End Function
                'Optional - More Functions and Subs
                'Optional - End Class
                'Optional - More Classes
                'Optional - End Namespace
                'Optional - More Namespaces
                Dim strFUNCTIONS As String = mt
                Dim strCLASSES As String = mt
                Dim strNAMESPACES As String = mt
                If ContainingText(strSOURCE_CODE.ToUpper, "END FUNCTION") Then
                    strFUNCTIONS = Mid(strSOURCE_CODE, InStr(strSOURCE_CODE.ToUpper, "END FUNCTION") + "END FUNCTION".Length)
                    strSOURCE_CODE = Mid(strSOURCE_CODE, 1, InStr(strSOURCE_CODE.ToUpper, "END FUNCTION") - 1)
                End If 'intP
                If ContainingText(strFUNCTIONS.ToUpper, "END CLASS") Then
                    strCLASSES = Mid(strFUNCTIONS, InStr(strFUNCTIONS.ToUpper, "END CLASS") + "END CLASS".Length)
                    strFUNCTIONS = Mid(strFUNCTIONS, 1, InStr(strFUNCTIONS.ToUpper, "END CLASS") - 1)
                End If 'intP
                If ContainingText(strCLASSES.ToUpper, "END NAMESPACE") Then
                    strNAMESPACES = Mid(strCLASSES, InStr(strCLASSES.ToUpper, "END NAMESPACE") + "END NAMESPACE".Length)
                    strCLASSES = Mid(strCLASSES, 1, InStr(strCLASSES.ToUpper, "END NAMESPACE") - 1)
                End If 'intP

                If stpCODE_FILE.Length = 0 Then
                    stpCODE_FILE.dLine("Option Infer On")
                    stpCODE_FILE.dLine("Imports System")
                    stpCODE_FILE.dLine("Imports Microsoft.VisualBasic")
                    'sb.dLine("Imports System.Xml")
                    'sb.dLine("Imports System.Data")
                End If 'sb

                ' Build a little wrapper code, with our passed in code in the middle 
                stpCODE_FILE.dLine("Namespace Mx")
                stpCODE_FILE.dLine("Class Class1")
                stpCODE_FILE.dLine("Public Shared Function SourcePath() As String")
                stpCODE_FILE.dLine(Strapd().d("SourcePath = " & qs & "@r1" & qs).r1(FileNamed().d(ur_assembly_folder).d(ur_input_script)))
                stpCODE_FILE.dLine("End Function")

                stpCODE_FILE.dLine(mt)
                stpCODE_FILE.dLine("Public Shared Function SourceFolder() As String")
                stpCODE_FILE.dLine("SourceFolder = System.IO.Path.GetDirectoryName(Mx.Class1.SourcePath)")
                stpCODE_FILE.dLine("End Function")

                stpCODE_FILE.dLine(mt)
                If objCOMPILER_PARM.ReferencedAssemblies.Count > 1 Then
                    stpCODE_FILE.dLine("Public Shared Function prvMyResolveEventHandler(Optional sender As Object = Nothing, Optional args As System.ResolveEventArgs = Nothing) As System.Reflection.Assembly")
                    stpCODE_FILE.dLine("    prvMyResolveEventHandler = Nothing")
                    stpCODE_FILE.dLine("    If args Is Nothing Then")
                    stpCODE_FILE.dLine("        RemoveHandler System.AppDomain.CurrentDomain.AssemblyResolve, AddressOf prvMyResolveEventHandler")
                    stpCODE_FILE.dLine("        AddHandler System.AppDomain.CurrentDomain.AssemblyResolve, AddressOf prvMyResolveEventHandler")
                    stpCODE_FILE.dLine(mt)
                    stpCODE_FILE.dLine("    Else 'args")
                    stpCODE_FILE.dLine("        Using stmCONFIG_FILE = New System.IO.StreamReader(Mx.Class1.SourcePath, New System.Text.UTF8Encoding(True, True))")
                    stpCODE_FILE.dLine("            While stmCONFIG_FILE.EndOfStream = False")
                    stpCODE_FILE.dLine("                Dim strENTRY = stmCONFIG_FILE.ReadLine")
                    stpCODE_FILE.dLine(Strapd().d("                If String.Equals(Left(strENTRY, Len(@r1start@r1 & @r1 @r1)), @r1start@r1 & @r1 @r1, System.StringComparison.CurrentCultureIgnoreCase) OrElse").r1(qs))
                    stpCODE_FILE.dLine(Strapd().d("                String.Equals(Left(strENTRY, Len(@r1exit@r1)), @r1exit@r1, System.StringComparison.CurrentCultureIgnoreCase) Then").r1(qs))
                    stpCODE_FILE.dLine(Strapd().d("                ElseIf String.Equals(Right(strENTRY, Len(@r1.DLL@r1)), @r1.DLL@r1, System.StringComparison.CurrentCultureIgnoreCase) Then").r1(qs))
                    stpCODE_FILE.dLine(Strapd().d("                    If InStr(strENTRY, @r1\@r1) = 0 Then").r1(qs))
                    stpCODE_FILE.dLine("                        strENTRY = System.IO.Path.Combine(Mx.Class1.SourcePath, strENTRY)")
                    stpCODE_FILE.dLine("                    End If 'strENTRY")
                    stpCODE_FILE.dLine("                    ")
                    stpCODE_FILE.dLine("                    If String.Equals(Left(args.Name, Len(System.IO.Path.GetFileNameWithoutExtension(strENTRY))), System.IO.Path.GetFileNameWithoutExtension(strENTRY), System.StringComparison.CurrentCultureIgnoreCase) Then")
                    stpCODE_FILE.dLine("                        prvMyResolveEventHandler = System.Reflection.Assembly.LoadFrom(strENTRY)")
                    stpCODE_FILE.dLine("                        Exit While")
                    stpCODE_FILE.dLine("                    End If 'args")
                    stpCODE_FILE.dLine(mt)
                    stpCODE_FILE.dLine("                Else 'strENTRY")
                    stpCODE_FILE.dLine("                    Exit While")
                    stpCODE_FILE.dLine("                End If 'strENTRY")
                    stpCODE_FILE.dLine("            End While 'stmCONFIG_FILE")
                    stpCODE_FILE.dLine("        End Using 'stmCONFIG_FILE")
                    stpCODE_FILE.dLine("    End If 'args")
                    stpCODE_FILE.dLine("End Function 'prvMyResolveEventHandler")
                End If 'cp

                stpCODE_FILE.dLine(mt)
                stpCODE_FILE.dLine("Public Shared Function RetVal() As String")
                stpCODE_FILE.dLine("RetVal = " & qs & qs)
                If objCOMPILER_PARM.ReferencedAssemblies.Count > 1 Then
                    stpCODE_FILE.dLine("Call prvMyResolveEventHandler()")
                End If 'cp

                stpCODE_FILE.dLine("Try")
                stpCODE_FILE.dLine(strSOURCE_CODE)
                stpCODE_FILE.dLine("Catch ex As System.Exception")
                stpCODE_FILE.dLine("RetVal = RetVal & Microsoft.VisualBasic.Constants.vbCrLf & ex.Message")
                stpCODE_FILE.dLine("End Try")
                stpCODE_FILE.dLine("End Function")

                stpCODE_FILE.dLine(mt)
                stpCODE_FILE.dLine(strFUNCTIONS)
                stpCODE_FILE.dLine("End Class")
                stpCODE_FILE.dLine(strCLASSES)
                stpCODE_FILE.dLine("End Namespace")
                stpCODE_FILE.dLine(strNAMESPACES)
                For Each strENTRY In sdaEXTERNAL_FILES
                    If System.IO.File.Exists(strENTRY) Then
                        stpCODE_FILE.dLine(My.Computer.FileSystem.ReadAllText(strENTRY).Replace("Option Infer On", mt).Replace("Option Strict On", mt))

                    Else
                        stpERR_MSG.dLine(Strapd().d("Referenced file is missing").dSprtr(": ", strENTRY))
                    End If
                Next strENTRY
            End Using 'stmFILE

            If stpERR_MSG.Length = 0 Then
                ' Invoke compilation.
                Dim provider As System.CodeDom.Compiler.CodeDomProvider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider(ur_provider.name) 'New Microsoft.VisualBasic.VBCodeProvider 
                objCOPMILER_RESULT = provider.CompileAssemblyFromSource(objCOMPILER_PARM, stpCODE_FILE.ToString)
                If objCOPMILER_RESULT.Errors.Count <> 0 Then
                    stpERR_MSG.dLine("Compile Errors")
                    stpERR_MSG.dLine()
                    For Each errItem As System.CodeDom.Compiler.CompilerError In objCOPMILER_RESULT.Errors
                        stpERR_MSG.dLine(errItem.ErrorText & " [" & errItem.Line + 5 & "]")
                        stpERR_MSG.dLine()
                        stpERR_MSG.dLine()
                        stpERR_MSG.dLine(stpCODE_FILE.ToString)
                    Next errItem
                End If 'cr
            End If

            If stpERR_MSG.Length = 0 Then
                Dim t As System.Type = objCOPMILER_RESULT.CompiledAssembly.CreateInstance("Mx.Class1").GetType()
                For Each m As System.Reflection.MethodInfo In t.GetMethods
                    If m.Name = "RetVal" Then
                        objMETHOD_MAIN = m
                        Exit For
                    End If
                Next m

                If objMETHOD_MAIN IsNot Nothing Then
                    Dim objRESULT = objMETHOD_MAIN.Invoke(Nothing, Nothing)
                    If objRESULT IsNot Nothing Then
                        stpERR_MSG.dLine(objRESULT.ToString)
                    End If 'objRESULT
                End If 'mthMAIN
            End If

            ' Return the results of compilation.
            Return stpERR_MSG.ToString
        End Function 'RunCode
    End Class 'ScriptRun
End Namespace 'Mx
