Namespace Mx
    Partial Public Class MxText
        Public Shared Function ToAlphaCommaNumber(ur_num As System.Numerics.BigInteger) As String
            Dim stpRET = Strapd()
            Dim strNUM = ur_num.ToString
            Dim intNUM_LEN = strNUM.Length
            Dim intPOS = 1
            If Left(strNUM, 1) = "-" Then
                intPOS = 2
                intNUM_LEN -= 1
                stpRET.d("-")
            End If

            Dim intPARTIAL_GROUP = intNUM_LEN Mod 3
            Dim strLEAD_PAD = ""
            If intPARTIAL_GROUP > 0 Then
                strLEAD_PAD = Space(3 - intPARTIAL_GROUP).Replace(" "c, "0"c)
                intNUM_LEN -= intPARTIAL_GROUP
            Else
                intNUM_LEN -= 3
                intPARTIAL_GROUP = 3
            End If

            Dim intGROUP = Math.Truncate(intNUM_LEN / 3) Mod 21
            Dim intREPEAT = Math.Truncate(Math.Truncate(intNUM_LEN / 3) / 21)
            For RPTCTR = intREPEAT To 0 Step -1
                Dim strREPT = Space(RPTCTR).Replace(" "c, "z"c)

                For GRPCTR = b1(intGROUP) To 1 Step -1
                    stpRET.d(strREPT)
                    Dim strGROUP = ChrW(AscW("d"c) + GRPCTR)
                    stpRET.d(strGROUP)
                    If intPARTIAL_GROUP < 3 Then
                        stpRET.d(strLEAD_PAD)
                    End If

                    stpRET.d(Mid(strNUM, intPOS, intPARTIAL_GROUP))
                    intPOS += intPARTIAL_GROUP
                    intPARTIAL_GROUP = 3
                Next GRPCTR

                If intGROUP < 20 Then
                    intGROUP = 20
                End If
            Next RPTCTR

            ToAlphaCommaNumber = stpRET.ToString
        End Function 'ToAlphaCommaNumber
    End Class 'MxText
End Namespace