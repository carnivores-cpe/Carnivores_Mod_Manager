Public Class Form1

    Dim dir, tempdir As String
    Dim debug As Boolean

    Const AreaTab = 0
    Const HuntableTab = 1
    Const AmbientTab = 2
    Const AmbientCorpseTab = 3
    Const MapAmbientTab = 4
    Const WeaponTab = 5
    Const EquipmentTab = 6
    Const OtherTab = 7

    Dim editForm As Form
    Dim editFormResult As Boolean

    Dim startMoney As Integer

    Dim tabs(7) As Tab '8 tabs

    Dim version(0) As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        init()
        'todo load dialog?
        setupTabData()
        loadData()
        drawTabs()

    End Sub

    Private Sub init()
        version(0) = "0.1.0"
        FileOpen(2, "MODMANAGER.LOG", OpenMode.Output)
        printLog("")
        debug = False
        startMoney = 0
    End Sub

    Private Sub setupTabData()

        tabs(AreaTab) = New Tab()
        tabs(AreaTab).name = "Areas"
        tabs(AreaTab).nameS = "Area"
        tabs(AreaTab).addImage = "Resources\add.png"
        tabs(AreaTab).editImage = "Resources\edit.png"
        tabs(AreaTab).removeImage = "Resources\delete.png"
        tabs(AreaTab).recordMax = 10
        tabs(AreaTab).recordLock = False
        tabs(AreaTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", "", True)

        tabs(1) = New Tab()
        tabs(HuntableTab).name = "Huntable Creatures"
        tabs(HuntableTab).nameS = "Huntable Creature"
        tabs(HuntableTab).addImage = "Resources\addhuntable.png"
        tabs(HuntableTab).editImage = "Resources\edit.png"
        tabs(HuntableTab).removeImage = "Resources\deletehuntable.png"
        tabs(HuntableTab).recordMax = 10
        tabs(HuntableTab).recordLock = False
        tabs(HuntableTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", "", True)

        tabs(AmbientTab) = New Tab()
        tabs(AmbientTab).name = "Ambient Creatures"
        tabs(AmbientTab).nameS = "Ambient Creature"
        tabs(AmbientTab).addImage = "Resources\add.png"
        tabs(AmbientTab).editImage = "Resources\edit.png"
        tabs(AmbientTab).removeImage = "Resources\delete.png"
        tabs(AmbientTab).recordMax = 5
        tabs(AmbientTab).recordLock = False

        tabs(AmbientCorpseTab) = New Tab()
        tabs(AmbientCorpseTab).name = "Ambient Corpses"
        tabs(AmbientCorpseTab).nameS = "Ambient Corpse"
        tabs(AmbientCorpseTab).addImage = "Resources\add.png"
        tabs(AmbientCorpseTab).editImage = "Resources\edit.png"
        tabs(AmbientCorpseTab).removeImage = "Resources\delete.png"
        tabs(AmbientCorpseTab).recordMax = 4
        tabs(AmbientCorpseTab).recordLock = False

        tabs(MapAmbientTab) = New Tab()
        tabs(MapAmbientTab).name = "Map Ambient Creatures"
        tabs(MapAmbientTab).nameS = "Map Ambient Creature"
        tabs(MapAmbientTab).addImage = "Resources\add.png"
        tabs(MapAmbientTab).editImage = "Resources\edit.png"
        tabs(MapAmbientTab).removeImage = "Resources\delete.png"
        tabs(MapAmbientTab).recordMax = 1024
        tabs(MapAmbientTab).recordLock = False

        tabs(WeaponTab) = New Tab()
        tabs(WeaponTab).name = "Weapons"
        tabs(WeaponTab).nameS = "Weapon"
        tabs(WeaponTab).addImage = "Resources\add.png"
        tabs(WeaponTab).editImage = "Resources\edit.png"
        tabs(WeaponTab).removeImage = "Resources\delete.png"
        tabs(WeaponTab).recordMax = 10
        tabs(WeaponTab).recordLock = False
        tabs(WeaponTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("CAR File", "file", AttributeType.attrFile, "", 0, 509, False, "Temp\WeaponCar\", "\HUNTDAT\WEAPONS\", "car", True)
        tabs(WeaponTab).addAttribute("Bullet Image", "pic", AttributeType.attrFile, "", 0, 509, False, "Temp\Bullet\", "\HUNTDAT\WEAPONS\", "tga", True)
        tabs(WeaponTab).addAttribute("Gunshot Sound", "gunshot", AttributeType.attrFile, "", 0, 509, True, "Temp\Gunshot\", "\MULTIPLAYER\GUNSHOTS\", "wav", True)
        tabs(WeaponTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Power", "power", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Shots", "shots", AttributeType.attrInteger, 0, 1, 2147483647, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Reload", "reload", AttributeType.attrInteger, 0, 1, 2147483647, False, "", "", "", True) 'togglableint - if off, value is 0
        tabs(WeaponTab).addAttribute("Trace", "trace", AttributeType.attrInteger, 1, 1, 2147483647, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Precision", "prec", AttributeType.attrDouble, 0, 0, 2, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Volume", "loud", AttributeType.attrDouble, 0, 0, 2, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Rate of Fire", "rate", AttributeType.attrDouble, 0, 0, 2, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Scope Zoom", "optic", AttributeType.attrIntBool, False, 0, 0, False, "", "", "", True)
        tabs(WeaponTab).addAttribute("Bullet Fall", "fall", AttributeType.attrIntBool, False, 0, 0, False, "", "", "", True)

        tabs(EquipmentTab) = New Tab()
        tabs(EquipmentTab).name = "Equipment"
        tabs(EquipmentTab).nameS = "Equipment"
        tabs(EquipmentTab).addImage = "Resources\add.png"
        tabs(EquipmentTab).editImage = "Resources\edit.png"
        tabs(EquipmentTab).removeImage = "Resources\delete.png"
        tabs(EquipmentTab).recordMax = 4
        tabs(EquipmentTab).recordLock = True
        tabs(EquipmentTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", "", False)
        tabs(EquipmentTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", "", True)

        tabs(OtherTab) = New Tab()
        tabs(OtherTab).name = "Other"



    End Sub

    Private Sub drawTabs()

        For tabIndex As Integer = 0 To TabControl1.TabCount - 1

            TabControl1.TabPages(tabIndex).Text = tabs(tabIndex).name

            If tabIndex <> 7 Then 'not other tab

                tabs(tabIndex).addToolTip = New ToolTip
                tabs(tabIndex).addToolTip.ShowAlways = True
                tabs(tabIndex).addButton = New Button
                tabs(tabIndex).addButton.Image = New Bitmap(Image.FromFile(tabs(tabIndex).addImage), New Size(32, 32))
                tabs(tabIndex).addButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).addButton.Location = New Drawing.Point(366, 8)
                tabs(tabIndex).addToolTip.SetToolTip(tabs(tabIndex).addButton, "Add " & tabs(tabIndex).nameS)
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).addButton)

                tabs(tabIndex).editToolTip = New ToolTip
                tabs(tabIndex).editToolTip.ShowAlways = True
                tabs(tabIndex).editButton = New Button
                tabs(tabIndex).editButton.Image = New Bitmap(Image.FromFile(tabs(tabIndex).editImage), New Size(32, 32))
                tabs(tabIndex).editButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).editButton.Location = New Drawing.Point(366, 54)
                tabs(tabIndex).editButton.Enabled = False
                AddHandler tabs(tabIndex).editButton.Click, AddressOf editrecord
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).editButton)

                tabs(tabIndex).removeToolTip = New ToolTip
                tabs(tabIndex).removeToolTip.ShowAlways = True
                tabs(tabIndex).removeButton = New Button
                tabs(tabIndex).removeButton.Image = New Bitmap(Image.FromFile(tabs(tabIndex).removeImage), New Size(32, 32))
                tabs(tabIndex).removeButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).removeButton.Location = New Drawing.Point(366, 100)
                tabs(tabIndex).removeButton.Enabled = False
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).removeButton)

                tabs(tabIndex).listBox = New ListBox()
                tabs(tabIndex).listBox.Size = New Drawing.Size(350, 360)
                tabs(tabIndex).listBox.Location = New Drawing.Point(8, 8)
                For recordIndex As Integer = 0 To tabs(tabIndex).records.Count - 1
                    tabs(tabIndex).listBox.Items.Add(tabs(tabIndex).getAttr(recordIndex, "name"))
                Next

                AddHandler tabs(tabIndex).listBox.SelectedIndexChanged, AddressOf ListBoxControlUpdate

                ListBoxControlSetup(tabIndex)

                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).listBox)
            End If
        Next
    End Sub

    Private Sub addrecord()

        'If tabs(TabIndex).listBox.Items.Count > 0 Then tabs(TabIndex).listBox.SelectedIndex = 0
        'check max items
        'create new record
        'openEditForm( - "Add " tab().nameS as task)
        'add record to tab if editformresult = true
        ListBoxControlUpdate()
    End Sub

    Private Sub editrecord()
        openEditForm(tabs(TabControl1.SelectedIndex).records(tabs(TabControl1.SelectedIndex).listBox.SelectedIndex), tabs(TabControl1.SelectedIndex).attributeClasses, "Edit " & tabs(TabControl1.SelectedIndex).getAttr(tabs(TabControl1.SelectedIndex).listBox.SelectedIndex, "name"))
        ListBoxControlUpdate()
    End Sub

    Private Sub deleteRecord()
        'confirmation
        'remove from list box
        ListBoxControlUpdate()
    End Sub

    Private Sub ListBoxControlUpdate()
        ListBoxControlSetup(TabControl1.SelectedIndex)
    End Sub

    Private Sub ListBoxControlSetup(ByVal tabindex As Integer)
        If tabs(tabindex).listBox.Items.Count > 0 Then
            If tabs(tabindex).listBox.SelectedIndex = -1 Then
                tabs(tabindex).listBox.SelectedIndex = 0
            End If
        Else
            tabs(tabindex).listBox.SelectedIndex = -1
        End If
        If tabs(tabindex).recordLock = True Then
            tabs(tabindex).addButton.Enabled = False
            tabs(tabindex).editButton.Enabled = True
            tabs(tabindex).removeButton.Enabled = False
            tabs(tabindex).addButton.Visible = False
            tabs(tabindex).editButton.Visible = True
            tabs(tabindex).removeButton.Visible = False
            tabs(tabindex).editToolTip.SetToolTip(tabs(tabindex).editButton, "Edit " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
        Else
            If tabs(tabindex).records.Count = tabs(tabindex).recordMax Then
                tabs(tabindex).addButton.Enabled = False
            Else
                tabs(tabindex).addButton.Enabled = True
            End If
            If tabs(tabindex).records.Count = 0 Then
                tabs(tabindex).editButton.Enabled = False
                tabs(tabindex).removeButton.Enabled = False
            Else
                tabs(tabindex).editButton.Enabled = True
                tabs(tabindex).editToolTip.SetToolTip(tabs(tabindex).editButton, "Edit " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
                tabs(tabindex).removeToolTip.SetToolTip(tabs(tabindex).removeButton, "Delete " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
                tabs(tabindex).removeButton.Enabled = True
            End If
        End If
    End Sub

    Private Sub openEditForm(ByRef record As Record, ByRef attrclasses As List(Of AttributeClass), ByRef task As String)
        editForm = New Form
        editForm.Text = task
        Dim panel1 As Panel = New Panel
        editForm.Size = New Drawing.Size(300, 325)
        panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        panel1.Location = New Drawing.Point(0, 0)
        panel1.Size = New Drawing.Size(285, 250)
        panel1.AutoScroll = True
        editForm.FormBorderStyle = FormBorderStyle.FixedDialog
        editForm.MaximizeBox = False
        editForm.MinimizeBox = False
        Dim handle As List(Of Object)
        handle = New List(Of Object)
        Dim yPos As Integer = 8
        For attrIndex As Integer = 0 To record.attributes.Count - 1
            If attrclasses(attrIndex).hidden = False Then
                Dim label As New Label
                label.Size = New Drawing.Size(100, 15)
                label.Location = New Drawing.Point(8, yPos)
                label.Text = attrclasses(attrIndex).displayName
                panel1.Controls.Add(label)
                Select Case attrclasses(attrIndex).type

                    Case AttributeType.attrString
                        Dim textBox As TextBox = New TextBox
                        textBox.Size = New Drawing.Size(100, 15)
                        textBox.Location = New Drawing.Point(116, yPos)
                        textBox.Text = record.attributes(attrIndex).value
                        panel1.Controls.Add(textBox)
                        handle.Add(textBox)

                        If attrclasses(attrIndex).editable = False Then textBox.Enabled = False

                    'todo string max length

                    Case AttributeType.attrInteger
                        Dim numBox As NumericUpDown = New MyNumericUpDown 'custom class overrides scroll wheeling through options
                        numBox.Size = New Drawing.Size(100, 15)
                        numBox.Location = New Drawing.Point(116, yPos)
                        numBox.Text = record.attributes(attrIndex).value
                        numBox.Maximum = attrclasses(attrIndex).maxValue
                        numBox.Minimum = attrclasses(attrIndex).minValue
                        panel1.Controls.Add(numBox)
                        handle.Add(numBox)

                        If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                    Case AttributeType.attrDouble
                        Dim numBox As NumericUpDown = New MyNumericUpDown 'custom class overrides scroll wheeling through options
                        numBox.Size = New Drawing.Size(100, 15)
                        numBox.Location = New Drawing.Point(116, yPos)
                        numBox.Text = record.attributes(attrIndex).value
                        numBox.Maximum = attrclasses(attrIndex).maxValue
                        numBox.Minimum = attrclasses(attrIndex).minValue
                        numBox.DecimalPlaces = 2
                        numBox.Increment = 0.01D
                        panel1.Controls.Add(numBox)
                        handle.Add(numBox)

                        If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                    Case AttributeType.attrIntBool
                        Dim comboBox As ComboBox = New MyComboBox 'custom class overrides scroll wheeling through options
                        comboBox.Size = New Drawing.Size(100, 15)
                        comboBox.Location = New Drawing.Point(116, yPos)
                        comboBox.Text = record.attributes(attrIndex).value
                        comboBox.Items.Add("True")
                        comboBox.Items.Add("False")
                        If record.attributes(attrIndex).value = True Then
                            comboBox.SelectedIndex = 0
                        Else
                            comboBox.SelectedIndex = 1
                        End If
                        panel1.Controls.Add(comboBox)
                        handle.Add(comboBox)

                        If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                    Case AttributeType.attrBoolean
                        Dim comboBox As ComboBox = New MyComboBox 'custom class overrides scroll wheeling through options
                        comboBox.Size = New Drawing.Size(100, 15)
                        comboBox.Location = New Drawing.Point(116, yPos)
                        comboBox.Text = record.attributes(attrIndex).value
                        comboBox.Items.Add("True")
                        comboBox.Items.Add("False")
                        If record.attributes(attrIndex).value = True Then
                            comboBox.SelectedIndex = 0
                        Else
                            comboBox.SelectedIndex = 1
                        End If
                        panel1.Controls.Add(comboBox)
                        handle.Add(comboBox)

                        If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                    Case AttributeType.attrFile

                        Dim textBox As TextBox = New TextBox
                        textBox.Size = New Drawing.Size(78, 15)
                        textBox.Location = New Drawing.Point(116, yPos)
                        textBox.Text = record.attributes(attrIndex).value
                        textBox.Enabled = False
                        panel1.Controls.Add(textBox)
                        handle.Add(textBox)

                        Dim tooltip = New ToolTip
                        tooltip.ShowAlways = True
                        Dim button As LoadDataButton = New LoadDataButton
                        button.record = record
                        button.attrIndex = attrIndex
                        button.handle2 = textBox
                        button.Size = New Drawing.Size(23, 22)
                        button.Location = New Drawing.Point(194, yPos - 1)
                        button.Image = New Bitmap(Image.FromFile("Resources\openfile.png"), New Size(20, 20))
                        AddHandler button.Click, AddressOf OpenFile
                        tooltip.SetToolTip(button, "Open " & attrclasses(attrIndex).ext.ToUpper & " File")
                        panel1.Controls.Add(button)

                        If attrclasses(attrIndex).editable = False Then button.Enabled = False

                End Select

                Dim helptooltip = New ToolTip
                helptooltip.ShowAlways = True
                Dim helpButton As HelpButton = New HelpButton
                helpButton.Size = New Drawing.Size(22, 22)
                helpButton.Location = New Drawing.Point(228, yPos + 2)
                helpButton.Image = New Bitmap(Image.FromFile("Resources\help.png"), New Size(19, 19))
                helptooltip.SetToolTip(helpButton, "Help")
                helpButton.attrIndex = attrIndex
                AddHandler helpButton.Click, AddressOf showHelp
                panel1.Controls.Add(helpButton)

                yPos += 22
            Else
                handle.Add(Nothing)
            End If
        Next

        editForm.Controls.Add(panel1)

        Dim buttonOk As New Button
        buttonOk.Size = New Drawing.Size(60, 28)
        buttonOk.Location = New Drawing.Point(145, 254)
        buttonOk.Text = "Ok"
        AddHandler buttonOk.Click, AddressOf editFormOk
        editForm.Controls.Add(buttonOk)

        Dim buttonCancel As New Button
        buttonCancel.Size = New Drawing.Size(60, 28)
        buttonCancel.Location = New Drawing.Point(205, 254)
        buttonCancel.Text = "Cancel"
        AddHandler buttonCancel.Click, AddressOf editFormCancel
        editForm.Controls.Add(buttonCancel)

        editFormResult = False ' defualt value if user quits using X button

        editForm.ShowDialog()

        If editFormResult = True Then
            For attrIndex As Integer = 0 To record.attributes.Count - 1
                If attrclasses(attrIndex).hidden = False Then
                    Select Case attrclasses(attrIndex).type
                        Case AttributeType.attrString
                            record.attributes(attrIndex).value = handle(attrIndex).Text
                        Case AttributeType.attrFile
                            record.attributes(attrIndex).value = handle(attrIndex).Text
                        Case AttributeType.attrInteger
                            'todo - rest of these
                            record.attributes(attrIndex).value = handle(attrIndex).Text
                        Case AttributeType.attrDouble
                            'todo - rest of these
                            record.attributes(attrIndex).value = handle(attrIndex).Text
                        Case AttributeType.attrIntBool
                            If handle(attrIndex).selectedIndex = 0 Then
                                record.attributes(attrIndex).value = True
                            Else
                                record.attributes(attrIndex).value = False
                            End If
                        Case AttributeType.attrBoolean
                            If handle(attrIndex).selectedIndex = 0 Then
                                record.attributes(attrIndex).value = True
                            Else
                                record.attributes(attrIndex).value = False
                            End If
                    End Select
                End If
            Next

            'update name in listbox
            Dim ri As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
            tabs(TabControl1.SelectedIndex).listBox.Items.RemoveAt(ri)
            tabs(TabControl1.SelectedIndex).listBox.Items.Insert(ri, tabs(TabControl1.SelectedIndex).getAttr(ri, "name"))
            tabs(TabControl1.SelectedIndex).listBox.SelectedIndex -= 1

        End If

    End Sub

    Private Sub showHelp(sender As Object, e As EventArgs)
        Dim attrClass As AttributeClass = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrIndex)
        Dim helpForm As Form = New Form
        helpForm.Text = "Help"
        helpForm.FormBorderStyle = FormBorderStyle.FixedDialog
        helpForm.MaximizeBox = False
        helpForm.MinimizeBox = False
        Dim nameLabel As Label = New Label
        nameLabel.Text = attrClass.displayName & ":"
        nameLabel.Size = New Drawing.Size(190, 12)
        nameLabel.Location = New Drawing.Point(10, 5)
        helpForm.Controls.Add(nameLabel)
        Dim yPos As Integer = 20
        FileOpen(7, "HInfo/" & TabControl1.SelectedIndex & "_" & attrClass.resName, OpenMode.Input)
        While Not EOF(7)
            Dim label As Label = New Label
            label.Text = LineInput(7)
            label.Size = New Drawing.Size(200, 14)
            label.Location = New Drawing.Point(10, yPos)
            helpForm.Controls.Add(label)
            yPos += 14
        End While
        FileClose(7)
        helpForm.Size = New Drawing.Size(200, yPos + 45)
        helpForm.ShowDialog()
    End Sub

    Private Sub OpenFile(sender As Object, e As EventArgs)
        Dim dialog As New OpenFileDialog()
        Dim ext As String = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrindex).ext
        Dim tempFolder As String = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrindex).tempFolder
        Dim gameFolder As String = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrindex).gameFolder
        dialog.InitialDirectory = tempdir
        dialog.Filter = ext & " files (*." & ext & ")|*." & ext
        dialog.FilterIndex = 0
        dialog.RestoreDirectory = True  'what does this do again??
        If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            'check if file exists in temp or game folder - if not auto rename
            Dim filename As String = System.IO.Path.GetFileName(dialog.FileName)
            If OpenFileChecker(tempFolder, gameFolder, filename) = True Then
                Dim fileno As Integer = 1
                Do
                    fileno += 1
                    filename = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) & " (" & fileno & ").car"
                Loop Until Not OpenFileChecker(tempFolder, gameFolder, filename)
            End If

            'copy file to temp folder
            FileSystem.FileCopy(System.IO.Path.GetFullPath(dialog.FileName), tempFolder & filename)

            'update textbox
            sender.handle2.text = filename

            tempdir = System.IO.Path.GetFullPath(dialog.FileName)

        End If
    End Sub

    Private Function OpenFileChecker(tempFolder, gamefolder, filename)
        If My.Computer.FileSystem.FileExists(tempFolder & filename) Then Return True
        If My.Computer.FileSystem.FileExists(dir & gamefolder & filename) Then Return True
        Return False
    End Function

    Friend Class HelpButton
        Inherits PictureBox

        Public attrIndex As Integer

    End Class

    Friend Class LoadDataButton
        Inherits Button

        Public record As Record
        Public attrIndex As Integer
        Public handle2 As TextBox

    End Class

    Friend Class MyComboBox
        Inherits ComboBox

        Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
            Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
            mwe.Handled = True
        End Sub
    End Class

    Friend Class MyNumericUpDown
        Inherits NumericUpDown

        Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
            Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
            mwe.Handled = True
        End Sub
    End Class


    Private Sub editFormOk()
        editFormResult = True
        editForm.Close()
    End Sub

    Private Sub editFormCancel()
        editForm.Close()
    End Sub


    Private Sub shutDown()
        FileClose(2)
        End
    End Sub

    Public Sub loadData()

        Dim commandArgs As String() = Environment.GetCommandLineArgs()

        If commandArgs.Length < 1 Then DoHalt("No Carnivores folder selected")

        For argIndex As Integer = 0 To commandArgs.Length - 1
            If commandArgs(argIndex).Contains("dir=") Then
                dir = commandArgs(argIndex).Substring(4)
            End If
            If commandArgs(argIndex) = "-debug" Then debug = True
            'If commandArgs(argIndex) = "-debug" Then debug = True
        Next

        tempdir = dir

        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(0, "name", "Camoflauge")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(1, "name", "Radar")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(2, "name", "Cover Scent")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(3, "name", "Double Ammo")

        readRes()

    End Sub

    Private Sub printLog(ByVal txt As String)
        PrintLine(2, txt)
        Console.WriteLine(txt)
    End Sub

    Private Sub DoHalt(ByVal mess As String)
        MsgBox(mess, vbOKOnly, "Error")
        shutDown()
    End Sub

    Private Sub readRes()
        'this should be reshunt unless you fix up rexhunters menu
        'gotta print to res and reshunt
        If Not My.Computer.FileSystem.FileExists(dir & "\HUNTDAT\_RESHUNT.TXT") Then
            DoHalt("_RESHUNT.TXT not found")
        End If
        FileOpen(1, dir & "\HUNTDAT\_RESHUNT.TXT", OpenMode.Input)

        Dim line As String = LineInput(1)
        Do
            If line.Contains("weapons {") Then readData(WeaponTab, line)
            'If line.Contains("characters {") Then readCharacters(line)
            'If line.Contains("mapambients {") Then readMapAmbients(line)
            If line.Contains("prices {") Then readPrices(line)
            line = LineInput(1)
        Loop Until line = "."

        FileClose(1)
    End Sub

    Private Sub readData(ByVal tabIndex As Integer, ByVal line As String)
        Do
            If line.Contains("{") Then
                tabs(tabIndex).addRecord()
                Do
                    If line.Contains("=") Then
                        Dim resName As String = Trim(line.Substring(0, line.IndexOf("=")))
                        Dim value
                        Select Case tabs(tabIndex).getAttrType(resName)
                            Case AttributeType.attrString
                                value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                            Case AttributeType.attrFile
                                value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                            Case AttributeType.attrInteger
                                value = Trim(line.Substring(line.IndexOf("=") + 1))
                            Case AttributeType.attrDouble
                                value = Trim(line.Substring(line.IndexOf("=") + 1))
                            Case AttributeType.attrBoolean
                                If line.Contains("TRUE") Then value = True Else value = False
                            Case AttributeType.attrIntBool
                                If Trim(line.Substring(line.IndexOf("=") + 1)) > 0 Then value = True Else value = False
                        End Select
                        tabs(tabIndex).setAttr(tabs(tabIndex).records.Count - 1, resName, value)
                    End If
                    line = LineInput(1)
                Loop Until line.Contains("}")
                If debug Then
                    printLog("READ " & tabs(tabIndex).nameS & " : " & tabs(tabIndex).records.Count - 1)
                    For attrIndex As Integer = 0 To tabs(tabIndex).attributeClasses.Count - 1
                        printLog(tabs(tabIndex).attributeClasses(attrIndex).displayName & "=" & tabs(tabIndex).records(tabs(tabIndex).records.Count - 1).attributes(attrIndex).value)
                    Next
                    printLog("---------------------------------------------------------------------")
                End If
            End If
            line = LineInput(1)
        Loop Until line.Contains("}") Or tabs(tabIndex).records.Count = tabs(tabIndex).recordMax
    End Sub

    Private Sub readCharacters(ByVal line As String)

    End Sub

    Private Sub readMapAmbients(ByVal line As String)

    End Sub

    Private Sub readPrices(ByVal line As String)
        Dim w, h, a, e As Integer
        w = 0
        h = 0
        a = 0
        e = 0
        Do
            If line.Contains("start") Then
                startMoney = Trim(line.Substring(line.IndexOf("=") + 1))
            ElseIf line.Contains("area") Then
                'tabs(AreaTab).setAttr(a, "price", Trim(line.Substring(line.IndexOf("=") + 1)))
                'a += 1
            ElseIf line.Contains("dino") Then
                'tabs(HuntableTab).setAttr(h, "price", Trim(line.Substring(line.IndexOf("=") + 1)))
                'h += 1
            ElseIf line.Contains("weapon") Then
                tabs(WeaponTab).setAttr(w, "price", Trim(line.Substring(line.IndexOf("=") + 1)))
                w += 1
            ElseIf line.Contains("acces") Then
                tabs(EquipmentTab).setAttr(e, "price", Trim(line.Substring(line.IndexOf("=") + 1)))
                e += 1
            End If
            line = LineInput(1)
        Loop Until line.Contains("}")



        If debug Then
            printLog("READ PRICES:")
            printLog("START MONEY = " & startMoney)
            If a Then
                For i As Integer = 0 To a - 1
                    printLog("AREA" & i & "=" & tabs(AreaTab).getAttr(i, "price"))
                Next
            End If
            If h Then
                For i As Integer = 0 To h - 1
                    printLog("HUNTABLE" & i & "=" & tabs(HuntableTab).getAttr(i, "price"))
                Next
            End If
            If w Then
                For i As Integer = 0 To w - 1
                    printLog("WEAPON" & i & "=" & tabs(WeaponTab).getAttr(i, "price"))
                Next
            End If
            If e Then
                For i As Integer = 0 To e - 1
                    printLog("AREA" & i & "=" & tabs(EquipmentTab).getAttr(i, "price"))
                Next
            End If
            printLog("---------------------------------------------------------------------")
            End If

    End Sub


    Public Class Tab
        Public name, nameS, addImage, editImage, removeImage As String
        Public addButton, removeButton, editButton As Button
        Public addToolTip, removeToolTip, editToolTip As ToolTip
        Public listBox As ListBox
        Public recordLock As Boolean ' prevent adding/deleting records

        Public attributeClasses As List(Of AttributeClass)
        Public recordMax As Integer
        Public records As List(Of Record)

        Public Sub New()
            records = New List(Of Record)
            attributeClasses = New List(Of AttributeClass)
        End Sub

        Public Sub addAttribute(ByVal name As String, ByVal res As String, ByVal type As AttributeType, ByVal defaultValue As Object,
                                ByVal min As Integer, ByVal max As Integer, ByVal hide As Boolean, ByVal tempFolder As String,
                                ByVal gameFolder As String, ByVal ext As String, ByVal edit As Boolean)
            attributeClasses.Add(New AttributeClass(name, res, type, defaultValue, min, max, hide, tempFolder, gameFolder, ext, edit))
            'clear temp folder
            If type = AttributeType.attrFile Then
                Dim path As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Remove(0, 6) & "\" & tempFolder & "\"
                For Each fi In New IO.DirectoryInfo(path).GetFiles("*." & ext)
                    My.Computer.FileSystem.DeleteFile(path & fi.Name)
                Next
            End If
        End Sub

        Public Sub addRecord()
            records.Add(New Record)
            records(records.Count - 1).attributes = New List(Of Attribute)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                records(records.Count - 1).attributes.Add(New Attribute(attributeClasses(attrIndex).defaultValue))
            Next
        End Sub

        Public Sub setAttr(ByVal recordIndex As Integer, ByVal resName As String, ByVal value As Object)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    records(recordIndex).attributes(attrIndex).value = value
                End If
            Next
        End Sub

        Public Function getAttr(ByVal recordIndex As Integer, ByVal resName As String)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    Return records(recordIndex).attributes(attrIndex).value
                End If
            Next
            Return -1
        End Function

        Public Function getAttrType(resName As String)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    Return attributeClasses(attrIndex).type
                End If
            Next
            Return -1
        End Function

    End Class

    Public Class Record
        'Dim name As String 'name of individual dino
        Public attributes As List(Of Attribute)
    End Class

    Public Class AttributeClass
        Public displayName As String
        Public resName As String
        Public type As AttributeType
        Public defaultValue
        Public maxValue, minValue As Integer
        Public hidden As Boolean
        Public editable As Boolean

        Public tempFolder, gameFolder, ext As String 'file data only

        Public Sub New(ByVal _name As String, ByVal _res As String, ByVal _type As AttributeType, ByVal defVal As Object,
                       ByVal min As Integer, ByVal max As Integer, ByVal _hidden As Boolean, ByVal _tempFolder As String,
                       ByVal _gameFolder As String, ByVal _ext As String, ByVal _edit As Boolean)
            displayName = _name
            resName = _res
            type = _type
            defaultValue = defVal
            minValue = min
            maxValue = max
            hidden = _hidden
            tempFolder = _tempFolder
            gameFolder = _gameFolder
            ext = _ext
            editable = _edit
        End Sub

    End Class

    Enum AttributeType
        attrString  'basic text
        attrInteger 'whole numbers
        attrDouble  'decimal numbers
        attrBoolean ' true/false
        attrIntBool ' true/false - written with numbers in the res

        attrFile
    End Enum

    Public Class Attribute
        Public value As Object
        Public Sub New(ByVal _value As Object)
            value = _value
        End Sub
        Public Sub setvalue(ByVal _value As Object)
            value = _value
        End Sub
    End Class

End Class
