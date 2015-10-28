Imports MySql.Data.MySqlClient

Public Class Form1

    Dim MySqlConn = New MySqlConnection
    Dim Command As MySqlCommand
    Dim Importe = 0

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim READER As MySqlDataReader
        Dim bSource As New BindingSource
        Dim Query As String
        Try
            MySqlConn.Open()

            Query = "insert into Maraja.clientes(NOMBRE,TELEFONO,DIRECCION,REPARTIDOR,ADEUDO,ULTIMOPAGO) values ('" & TxtNombre.Text & "','" & TxtTelefono.Text & "','" & TxtDireccion.Text & "','" & TextBox11.Text & "',0,'" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"


            Command = New MySqlCommand(Query, MySqlConn)
            READER = Command.ExecuteReader
            MessageBox.Show("Información Guardada")

            MySqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            MySqlConn.Dispose()
        End Try
        TxtNombre.Clear()
        TxtTelefono.Clear()
        TxtDireccion.Clear()
        TextBox11.Clear()


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim READER As MySqlDataReader
        Dim SDA As New MySqlDataAdapter
        Dim dbDataSet As New DataTable
        Dim bSource As New BindingSource
        Dim Query As String
        Try
            MySqlConn.Open()

            Query = "insert into Maraja.historial(Repartidor,CargaMedioLT,CargaLT,FiadoMedioLT,FiadoLT,VendidoMedioLT,VendidoLT,MermaMedioLT,MermaLitro,GananciaDiaria,FiadoTotal,GananciaReal,Fecha) values ('" & TxtRepartidor.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "','" & TextBox7.Text & "','" & TextBox1.Text & "','" & TextBox14.Text & "' ,'" & TextBox8.Text & "','" & TextBox9.Text & "','" & TextBox10.Text & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"

            Command = New MySqlCommand(Query, MySqlConn)
            READER = Command.ExecuteReader
            MessageBox.Show("Información Guardada")

            MySqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            MySqlConn.Dispose()
        End Try

        Try
            MySqlConn.Open()
            Query = "Select * from Maraja.historial where Fecha = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "'"
            Command = New MySqlCommand(Query, MySqlConn)
            SDA.SelectCommand = Command
            SDA.Fill(dbDataSet)
            bSource.DataSource = dbDataSet
            DataGridView2.DataSource = bSource
            SDA.Update(dbDataSet)

            MySqlConn.Close()
        Catch ex As Exception
            MessageBox.Show("Error")
        Finally
            MySqlConn.Dispose()
        End Try

        TxtRepartidor.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        TextBox9.Clear()
        TextBox10.Clear()
        TextBox1.Clear()
        TextBox14.Clear()



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim SDA As New MySqlDataAdapter
        Dim dbDataSet As New DataTable
        Dim bSource As New BindingSource
        Try
            MySqlConn.Open()
            Dim Query As String
            If TxtRepartidor2.Text = String.Empty Then
                Query = "select * from Maraja.CLIENTES"
            Else
                Query = "Select * from Maraja.Clientes where Repartidor = '" & TxtRepartidor2.Text & "' ;"
            End If

            Command = New MySqlCommand(Query, MySqlConn)
            SDA.SelectCommand = Command
            SDA.Fill(dbDataSet)
            bSource.DataSource = dbDataSet
            DataGridView2.DataSource = bSource
            SDA.Update(dbDataSet)
            MySqlConn.Close()
        Catch ex As Exception
            MessageBox.Show("No existen registros con esos parametros")
        Finally
            MySqlConn.Dispose()
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        'Dim READER As MySqlDataReader
        Dim SDA As New MySqlDataAdapter
        Dim dbDataSet As New DataTable
        Dim bSource As New BindingSource
        Dim Query, Query2, Query3 As String
        Dim mySet As String = "MiDataSet"
        Dim Tabladatos As New DataSet
        Dim MontoNota, MontoAbono, MontoAdeudo, MontoAUX As Integer
        Dim FechaNota As Date
        Dim Control As Boolean

        Try



            If TextBox12.Text = String.Empty Then
                'En caso de que sea un ADEUDO mas compruebo que no exista una nota con MONTO NEGATIVO
                'Busco La nota pendiente mas vieja y almaceno datos necesarios en variables.
                Try
                    Tabladatos = getDataSet("Select * from NOTASPENDIENTES where Nombre ='" & TxtCliente.Text & "'  group by Fecha", mySet)
                    MontoNota = Tabladatos.Tables(0).Rows(0).Item(1)
                    FechaNota = Tabladatos.Tables(0).Rows(0).Item(2)
                    MontoAdeudo = CInt(TextBox13.Text)
                    'Compruebo si existe una nota con monto negativo
                    If MontoNota < 0 Then
                        MontoAUX = MontoNota + MontoAdeudo
                        If MontoAUX <> 0 Then
                            'En caso de que el cliente siga teniendo ya se aun ADEUDO o un CREDITO A SU FAVOR actualizo la informacion de la tabla NOTASPENDIENTES y su ADEUDO general.
                            Query = "Update NOTASPENDIENTES set Monto = '" & MontoAUX & "' where Nombre = '" & TxtCliente.Text & "' and Fecha = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "';"
                            Query2 = "update Clientes set ADEUDO = '" & MontoAUX & "', ultimopago = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "' where nombre = '" & TxtCliente.Text & "';"
                            Consulta(Query)
                            Consulta(Query2)
                        Else
                            'En caso de que el credito a su favor sea igual al nuevo abono la deuda del cliente se elimina
                            Query = "Delete from NOTASPENDIENTES where Nombre = '" & TxtCliente.Text & "' and Monto = '" & MontoNota & "' and Fecha = '" & Format(CDate(FechaNota), "yyyy/MM/dd") & "';"
                            Query2 = "update Clientes set ADEUDO = 0, ultimopago = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "' where nombre = '" & TxtCliente.Text & "';"
                            Consulta(Query)
                            Consulta(Query2)
                        End If
                    Else
                        'En caso de que el cliente no tenga notas negativas le agrego una nueva NOTAPENDIENTE y aumento su ADEUDO
                        Query = "update Clientes set ADEUDO = ADEUDO + '" & TextBox13.Text & "', ultimopago = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "' where nombre = '" & TxtCliente.Text & "';"
                        Query2 = "insert into Maraja.NOTASPENDIENTES(Nombre,Monto,Fecha) values('" & TxtCliente.Text & "','" & TextBox13.Text & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"
                        Consulta(Query)
                        Consulta(Query2)

                    End If
                Catch
                    'En caso de que el cliente no tenia ningun ADEUDO le agrego una nueva NOTAPENDIENTE y aumento su ADEUDO
                    Query = "update Clientes set ADEUDO = ADEUDO + '" & TextBox13.Text & "', ultimopago = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "' where nombre = '" & TxtCliente.Text & "';"
                    Query2 = "insert into Maraja.NOTASPENDIENTES(Nombre,Monto,Fecha) values('" & TxtCliente.Text & "','" & TextBox13.Text & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"
                    Consulta(Query)
                    Consulta(Query2)

                End Try
            ElseIf TextBox13.Text = String.Empty
                'En caso de que sea un ABONO
                MontoAbono = CInt(TextBox12.Text)
                Control = True
                While Control = True
                    Try
                        'Busco La nota pendiente mas vieja y almaceno datos necesarios en variables.
                        Tabladatos = getDataSet("Select * from NOTASPENDIENTES where Nombre ='" & TxtCliente.Text & "'  group by Fecha", mySet)
                        MontoNota = Tabladatos.Tables(0).Rows(0).Item(1)
                        FechaNota = Tabladatos.Tables(0).Rows(0).Item(2)
                        MontoAUX = MontoNota - MontoAbono
                        If MontoNota < 0 Then
                            'Si la NOTAPENDIENTE tiene un valor negativo significa que este cliente tiene 
                            Query = "Update NOTASPENDIENTES set Monto = '" & MontoAUX & "' where Nombre = '" & TxtCliente.Text & "' and Fecha = '" & Format(CDate(FechaNota), "yyyy/MM/dd") & "';"
                            Consulta(Query)
                            Exit While
                        End If

                    Catch ex As Exception
                        'Si entra aqui es porque este cliente no tiene notas pendientes: inserto una nota negativa
                        Query = "insert into NOTASPENDIENTES(Nombre,Monto,Fecha) values ('" & TxtCliente.Text & "',-'" & MontoAbono & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"
                        Query2 = "insert into Maraja.NOTASPAGADAS(Nombre,Monto,Fecha) values('" & TxtCliente.Text & "','" & TextBox12.Text & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"
                        Consulta(Query)
                        Consulta(Query2)
                        Exit While
                    End Try
                    If MontoAUX > 0 Then
                        'Si el ABONO fue MENOR que la DEUDA: Actualizo la informacion de la nota con el nuevo monto
                        'Query = String.Format("Update NOTASPENDIENTES set ADEUDO = {0} where Nombre = '{1}' and Fecha = {3}", CInt(MontoAUX.ToString), TxtCliente, FechaNota)
                        Query = "Update NOTASPENDIENTES set Monto = '" & MontoAUX & "' where Nombre = '" & TxtCliente.Text & "' and Fecha = '" & Format(CDate(FechaNota), "yyyy/MM/dd") & "' Limit 1;"
                        Consulta(Query)
                        Control = False
                        MsgBox("Abono Menor")
                    ElseIf MontoAUX <= 0
                        'Si el ABONO fue MAYOR que la DEUDA: Borro la nota vieja, inserto una nueva NOTAPAGADA y le asigno el monto extra ala variable MONTOABONO. Repito el ciclo
                        Query = "Delete from NOTASPENDIENTES where Nombre = '" & TxtCliente.Text & "' and Monto = '" & MontoNota & "' and Fecha = '" & Format(CDate(FechaNota), "yyyy/MM/dd") & "' Limit 1;"
                        Query2 = "insert into Maraja.NOTASPAGADAS(Nombre,Monto,Fecha) values('" & TxtCliente.Text & "','" & TextBox12.Text & "','" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "');"
                        Consulta(Query)
                        Consulta(Query2)
                        If MontoAUX <> 0 Then
                            MontoAbono = -(MontoAUX)
                            Control = True
                        Else
                            Control = False
                        End If
                    End If
                End While
            End If

            Query = "update Clientes set ADEUDO = ADEUDO - '" & TextBox12.Text & "', ultimopago = '" & Format(CDate(Date.Now.ToShortDateString), "yyyy/MM/dd") & "' where nombre = '" & TxtCliente.Text & "';"
            Consulta(Query)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            MySqlConn.Dispose()
        End Try
        'Despliego informacion de los clientes
        Query = "Select * from Maraja.Clientes where Nombre = '" & TxtCliente.Text & "' ;"
        ConsultaDespliega(Query)
        TextBox12.Clear()
        TextBox13.Clear()
        TxtCliente.Clear()
    End Sub

    Private Sub BtnBorrar_Click(sender As Object, e As EventArgs) Handles BtnBorrar.Click
        Dim Query As String
        Query = "delete from Clientes where Nombre = '" & TxtNombre2.Text & "';"
        Consulta(Query)
        TxtNombre2.Clear()

    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox7.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox8_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox8.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox9.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox10.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox12_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox12.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TextBox13_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox13.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then     'No Letras
            If Asc(e.KeyChar) <> 8 Then
                e.Handled = True
            End If

        End If
    End Sub
    Private Sub TxtRepartidor_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TxtRepartidor.KeyPress
        If Not (e.KeyChar >= Chr(65) And e.KeyChar <= Chr(90)) And TxtRepartidor.SelectionStart = 0 Then e.Handled = True 'first letter must be uppercase
        If Not (e.KeyChar >= Chr(97) And e.KeyChar <= Chr(122)) And TxtRepartidor.SelectionStart > 0 Then e.Handled = True 'next letters must be lowercase
        If e.KeyChar = Chr(8) Then e.Handled = False 'allow Backspace
        If e.KeyChar = Chr(32) Then e.Handled = False ' allow Spacebar
    End Sub
    Private Sub TextBox3_LostFocus(sender As Object, e As EventArgs) Handles TextBox3.LostFocus
        Try
            Importe = (TextBox2.Text * 7.5) + (TextBox3.Text * 13)
            TextBox8.Text = Importe
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TextBox5_LostFocus(sender As Object, e As EventArgs) Handles TextBox5.LostFocus
        Try
            Importe = (TextBox4.Text * 7.5) + (TextBox5.Text * 13)
            TextBox9.Text = Importe
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TextBox7_LostFocus(sender As Object, e As EventArgs) Handles TextBox7.LostFocus
        Try
            Importe = (TextBox6.Text * 7.5) + (TextBox7.Text * 13)
            TextBox10.Text = Importe
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TextBox2_LostFocus(sender As Object, e As EventArgs) Handles TextBox2.LostFocus
        Try
            Importe = (TextBox2.Text * 7.5) + (TextBox3.Text * 13)
            TextBox8.Text = Importe
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TextBox4_LostFocus(sender As Object, e As EventArgs) Handles TextBox4.LostFocus
        Try
            Importe = (TextBox4.Text * 7.5) + (TextBox5.Text * 13)
            TextBox9.Text = Importe
        Catch ex As Exception

        End Try
    End Sub
    Private Sub TextBox6_LostFocus(sender As Object, e As EventArgs) Handles TextBox6.LostFocus
        Try
            Importe = (TextBox6.Text * 7.5) + (TextBox7.Text * 13)
            TextBox10.Text = Importe
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Consulta(Query As String)
        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim READER As MySqlDataReader
        Dim bSource As New BindingSource
        Try
            MySqlConn.Open()
            Command = New MySqlCommand(Query, MySqlConn)
            READER = Command.ExecuteReader
            MySqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            MySqlConn.Dispose()
        End Try

    End Sub
    Private Sub ConsultaDespliega(Query As String)
        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim SDA As New MySqlDataAdapter
        Dim dbDataSet As New DataTable
        Dim bSource As New BindingSource
        Try
            MySqlConn.Open()
            Command = New MySqlCommand(Query, MySqlConn)
            SDA.SelectCommand = Command
            SDA.Fill(dbDataSet)
            bSource.DataSource = dbDataSet
            DataGridView2.DataSource = bSource
            SDA.Update(dbDataSet)
            MySqlConn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            MySqlConn.Dispose()
        End Try
    End Sub
    Function getDataSet(Query As String, nombreDataSet As String) As DataSet
        MySqlConn.ConnectionString = "server=localhost;userid=root;password='';database=Maraja"
        Dim myAdapter As MySqlDataAdapter = New MySqlDataAdapter
        Dim MyDataSet As DataSet
        Try
            MySqlConn.Open()
            myAdapter = New MySqlDataAdapter(Query, MySqlConn.ConnectionString.ToString)
            MyDataSet = New DataSet
            myAdapter.Fill(MyDataSet, nombreDataSet)

        Catch

        Finally
            MySqlConn.Dispose()
        End Try
        Return MyDataSet
    End Function

    Private Sub BtnNotasPendientes_Click(sender As Object, e As EventArgs) Handles BtnNotasPendientes.Click
        Dim Query As String
        Query = ("Select * from NOTASPENDIENTES;")
        ConsultaDespliega(Query)
    End Sub

    Private Sub BtnNotasPagadas_Click(sender As Object, e As EventArgs) Handles BtnNotasPagadas.Click
        Dim Query As String
        Query = ("Select * from NOTASPAGADAS;")
        ConsultaDespliega(Query)

    End Sub


    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim Producto As String
        Dim Cantidad As Integer
        Dim Query As String
        If TextBox15.Text <> String.Empty Then
            Producto = ComBxProducto.Text
            Cantidad = CInt(TextBox15.Text)
            If RadioEntrada.Checked Then
                If RadMedioLT.Checked Then
                    Query = "Update INVENTARIO set Cantidad = Cantidad + '" & Cantidad & "' where producto = '" & Producto & " 1/2';"
                    Consulta(Query)
                ElseIf RadioLitro.Checked
                    Query = "Update INVENTARIO set Cantidad = Cantidad + '" & Cantidad & "' where Producto = '" & Producto & " L';"
                    Consulta(Query)
                End If
            ElseIf RadioSalida.Checked Then
                If RadMedioLT.Checked Then
                    Query = "Update INVENTARIO set Cantidad = Cantidad - '" & Cantidad & "' where producto = '" & Producto & " 1/2';"
                    Consulta(Query)
                ElseIf RadioLitro.Checked
                    Query = "Update INVENTARIO set Cantidad = Cantidad - '" & Cantidad & "' where Producto = '" & Producto & " L';"
                    Consulta(Query)
                End If
            End If
        Else
            MsgBox("Faltan datos")
        End If
        Query = "update inventario set Cantidad = 0 where Cantidad <= 0;"
        Consulta(Query)
        ActualizaPB()

    End Sub
    Private Sub ActualizaPB()
        Dim Query, Producto As String
        Dim Tabladatos As New DataSet
        Dim mySet As String = "MiDataSet"
        Dim X, Y As Integer
        Query = ("Select * from Maraja.INVENTARIO;")
        Tabladatos = getDataSet(Query, mySet)

        X = Tabladatos.Tables(0).Rows(0).Item(1)
        Y = Tabladatos.Tables(0).Rows(1).Item(1)
        Producto = X + Y
        Label41.Text = X
        Label42.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar1.Value = Producto

        X = Tabladatos.Tables(0).Rows(2).Item(1)
        Y = Tabladatos.Tables(0).Rows(3).Item(1)
        Producto = X + Y
        Label44.Text = X
        Label43.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar2.Value = Producto


        X = Tabladatos.Tables(0).Rows(4).Item(1)
        Y = Tabladatos.Tables(0).Rows(5).Item(1)
        Producto = X + Y
        Label46.Text = X
        Label45.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar3.Value = Producto


        X = Tabladatos.Tables(0).Rows(6).Item(1)
        Y = Tabladatos.Tables(0).Rows(7).Item(1)
        Producto = X + Y
        Label48.Text = X
        Label47.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar4.Value = Producto


        X = Tabladatos.Tables(0).Rows(8).Item(1)
        Y = Tabladatos.Tables(0).Rows(9).Item(1)
        Producto = X + Y
        Label50.Text = X
        Label49.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar5.Value = Producto


        X = Tabladatos.Tables(0).Rows(10).Item(1)
        Y = Tabladatos.Tables(0).Rows(11).Item(1)
        Producto = X + Y
        Label52.Text = X
        Label51.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar6.Value = Producto

        X = Tabladatos.Tables(0).Rows(12).Item(1)
        Y = Tabladatos.Tables(0).Rows(13).Item(1)
        Producto = X + Y
        Label54.Text = X
        Label53.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar7.Value = Producto


        X = Tabladatos.Tables(0).Rows(14).Item(1)
        Y = Tabladatos.Tables(0).Rows(15).Item(1)
        Producto = X + Y
        Label56.Text = X
        Label55.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar8.Value = Producto

        X = Tabladatos.Tables(0).Rows(16).Item(1)
        Y = Tabladatos.Tables(0).Rows(17).Item(1)
        Producto = X + Y
        Label58.Text = X
        Label57.Text = Y
        If Producto > 500 Then
            Producto = 500
        End If
        ProgressBar9.Value = Producto





    End Sub
End Class
