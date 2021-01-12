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

    Dim startMoney As Integer

    Dim tabs(7) As Tab '8 tabs

    Dim version(0) As String
    Dim modVersion As Integer
    Dim lastSupportedVersion As Integer
    Dim managerVersion As String = "1.0.0"

    Dim imgAdd As Bitmap = New Bitmap(Image.FromFile("Resources\add.png"), New Size(32, 32))
    Dim imgEdit As Bitmap = New Bitmap(Image.FromFile("Resources\edit.png"), New Size(32, 32))
    Dim imgDelete As Bitmap = New Bitmap(Image.FromFile("Resources\delete.png"), New Size(32, 32))
    Dim imgExpand As Bitmap = New Bitmap(Image.FromFile("Resources\expand.png"), New Size(20, 20))
    Dim imgOpenFile As Bitmap = New Bitmap(Image.FromFile("Resources\openfile.png"), New Size(20, 20))
    Dim imgHelp As Bitmap = New Bitmap(Image.FromFile("Resources\help.png"), New Size(19, 19))

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        init()
        'todo load dialog?
        setupTabData()
        loadData()
        drawTabs()

    End Sub

    Private Sub init()
        FileOpen(2, "MODMANAGER.LOG", OpenMode.Output)
        printLog("")
        debug = False
        startMoney = 0

    End Sub

    Private Sub setupTabData()

        tabs(AreaTab) = New Tab()
        tabs(AreaTab).name = "Areas"
        tabs(AreaTab).nameS = "Area"
        tabs(AreaTab).recordMax = 10
        tabs(AreaTab).recordMin = 0
        tabs(AreaTab).recordLock = False
        tabs(AreaTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, "The display name for the area in the hunt menu.")
        tabs(AreaTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\AREA", "txt", True, False, "The description for the area in the hunt menu.")
        tabs(AreaTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\AREA", "tga", True, True, "The image for the area in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(AreaTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, "The cost of hunting in the area.")
        tabs(AreaTab).addAttribute("Map File", "map&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\AREAS\AREA", "map", True, True, "The file that stores the terrain information, texture positions, object positions, sound positions, water and fog information.")
        tabs(AreaTab).addAttribute("Resource File", "rsc&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\AREAS\AREA", "rsc", True, True, "The file that stores textures, objects, animations, hitbox information and sounds for the map.")

        tabs(1) = New Tab()
        tabs(HuntableTab).name = "Huntable Creatures"
        tabs(HuntableTab).nameS = "Huntable Creature"
        tabs(HuntableTab).recordMax = 10
        tabs(HuntableTab).recordMin = 0
        tabs(HuntableTab).recordLock = False
        tabs(HuntableTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, "The cost of hunting the creature.")

        tabs(AmbientTab) = New Tab()
        tabs(AmbientTab).name = "Ambient Creatures"
        tabs(AmbientTab).nameS = "Ambient Creature"
        tabs(AmbientTab).recordMax = 5
        tabs(AmbientTab).recordLock = False

        tabs(AmbientCorpseTab) = New Tab()
        tabs(AmbientCorpseTab).name = "Ambient Corpses"
        tabs(AmbientCorpseTab).nameS = "Ambient Corpse"
        tabs(AmbientCorpseTab).recordMax = 4
        tabs(AmbientCorpseTab).recordLock = False

        tabs(MapAmbientTab) = New Tab()
        tabs(MapAmbientTab).name = "Map Ambient Creatures"
        tabs(MapAmbientTab).nameS = "Map Ambient Creature"
        tabs(MapAmbientTab).recordMax = 1024
        tabs(MapAmbientTab).recordLock = False

        tabs(WeaponTab) = New Tab()
        tabs(WeaponTab).name = "Weapons"
        tabs(WeaponTab).nameS = "Weapon"
        tabs(WeaponTab).recordMax = 10
        tabs(WeaponTab).recordMin = 0
        tabs(WeaponTab).recordLock = False
        tabs(WeaponTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, "The display name for the weapon in the menu.")
        tabs(WeaponTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\WEAPON", "txt", True, False, "The description for the weapon in the hunt menu.")
        tabs(WeaponTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\WEAPON", "tga", True, True, "The image for the weapon in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(WeaponTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, "The cost of using the weapon on a hunt.")
        tabs(WeaponTab).addAttribute("CAR File", "file", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\WEAPONS\", "car", True, True, "The file that stores the weapon model, texture, animations and sound effects.")
        tabs(WeaponTab).addAttribute("Bullet Image", "pic", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\WEAPONS\", "tga", True, True, "The bullet image used to display remaining ammo. Must be saved as a 16 bit uncompressed TGA.")
        tabs(WeaponTab).addAttribute("Gunshot Sound", "gunshot", AttributeType.attrFile, "", 0, 509, True, "\MULTIPLAYER\GUNSHOTS\", "wav", True, False, "The gunshot sound effect which other players can hear from a distance in multiplayer. Must be saved as a 22050 Hz rate mono WAV.")
        tabs(WeaponTab).addAttribute("Ammo Count", "shots", AttributeType.attrInteger, 1, 1, 2147483647, False, "", "", True, False, "The amount of ammo the hunter takes for this weapon on a hunt (Will be doubled if the hunter selects double ammo in equipment).")
        tabs(WeaponTab).addAttribute("Magazine Capacity", "reload", AttributeType.attrTogglableInteger, 0, 0, 2147483647, False, "", "", True, False, "The maximum number of rounds that can be fired before reloading. If set to default, the magazine capacity will equal the ammo count.") 'togglableint - if off, value is 0
        tabs(WeaponTab).addAttribute("Projectile Count", "trace", AttributeType.attrInteger, 1, 1, 2147483647, False, "", "", True, False, "The number of projectiles fired with each shot.")
        tabs(WeaponTab).addAttribute("Fire Power", "power", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, "The damage inflicted by the weapon. Note: the vanilla menu can only display values between 0 and 8.")
        tabs(WeaponTab).addAttribute("Accuracy", "prec", AttributeType.attrDouble, 0D, -10D, 2D, False, "", "", True, False, "Average bullet precision - 2.00 gives 100% accuracy. Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Volume", "loud", AttributeType.attrDouble, 0D, -10D, 10D, False, "", "", True, False, "The maximum distance creatures can hear gunshots (depending on wind direction). Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Rate of Fire", "rate", AttributeType.attrDouble, 0D, -10D, 10D, False, "", "", True, False, "The rate of fire for the weapon - this value only effects the menu stats and doesn't effect gameplay. Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Scope Zoom", "optic", AttributeType.attrIntBool, False, 0, 0, False, "", "", True, False, "When set to true, the hunter's screen will zoom in when the weapon is equipped.")
        tabs(WeaponTab).addAttribute("Bullet Fall", "fall", AttributeType.attrIntBool, False, 0, 0, False, "", "", True, False, "When set to true, projectile trajectory will drop over time.")

        tabs(EquipmentTab) = New Tab()
        tabs(EquipmentTab).name = "Equipment"
        tabs(EquipmentTab).nameS = "Equipment"
        tabs(EquipmentTab).recordMax = 4
        tabs(EquipmentTab).recordMin = 0
        tabs(EquipmentTab).recordLock = True
        tabs(EquipmentTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", False, True, "The display name for the equipment item in the menu.")
        tabs(EquipmentTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\EQUIP", "nfo", True, False, "The description for the equipment item in the hunt menu.")
        tabs(EquipmentTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\EQUIP", "tga", True, True, "The image for the equipment item in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(EquipmentTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, "The display name for the equipment item in the menu.")

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
                tabs(tabIndex).addButton.Image = imgAdd
                tabs(tabIndex).addButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).addButton.Location = New Drawing.Point(366, 8)
                tabs(tabIndex).addToolTip.SetToolTip(tabs(tabIndex).addButton, "Add " & tabs(tabIndex).nameS)
                AddHandler tabs(tabIndex).addButton.Click, AddressOf addrecord
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).addButton)

                tabs(tabIndex).editToolTip = New ToolTip
                tabs(tabIndex).editToolTip.ShowAlways = True
                tabs(tabIndex).editButton = New Button
                tabs(tabIndex).editButton.Image = imgEdit
                tabs(tabIndex).editButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).editButton.Location = New Drawing.Point(366, 54)
                tabs(tabIndex).editButton.Enabled = False
                AddHandler tabs(tabIndex).editButton.Click, AddressOf editrecord
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).editButton)

                tabs(tabIndex).removeToolTip = New ToolTip
                tabs(tabIndex).removeToolTip.ShowAlways = True
                tabs(tabIndex).removeButton = New Button
                tabs(tabIndex).removeButton.Image = imgDelete
                tabs(tabIndex).removeButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).removeButton.Location = New Drawing.Point(366, 100)
                tabs(tabIndex).removeButton.Enabled = False
                AddHandler tabs(tabIndex).removeButton.Click, AddressOf deleteRecord
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

        tabs(TabControl1.SelectedIndex).addRecord()
        'tabs(TabControl1.SelectedIndex).setAttr(tabs(TabControl1.SelectedIndex).records.Count - 1, "name", "test")

        openEditForm(tabs(TabControl1.SelectedIndex).records.Count - 1, tabs(TabControl1.SelectedIndex).attributeClasses, "New " & tabs(TabControl1.SelectedIndex).nameS)

        Dim name As String = tabs(TabControl1.SelectedIndex).getAttr(tabs(TabControl1.SelectedIndex).records.Count - 1, "name")

        If name.Trim = "" Then
            removeRecord(tabs(TabControl1.SelectedIndex).records.Count - 1)
        Else
            tabs(TabControl1.SelectedIndex).listBox.Items.Add(name)
            tabs(TabControl1.SelectedIndex).listBox.SelectedIndex = tabs(TabControl1.SelectedIndex).records.Count - 1
            ListBoxControlUpdate()
        End If
    End Sub

    Private Sub editrecord()
        openEditForm(tabs(TabControl1.SelectedIndex).listBox.SelectedIndex, tabs(TabControl1.SelectedIndex).attributeClasses, "Edit " & tabs(TabControl1.SelectedIndex).getAttr(tabs(TabControl1.SelectedIndex).listBox.SelectedIndex, "name"))
        ListBoxControlUpdate()
    End Sub

    Private Sub deleteRecord()
        Dim recordIndex As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
        If conf("Are you sure you want to delete " & tabs(TabControl1.SelectedIndex).getAttr(recordIndex, "name") & "?") Then
            removeRecord(recordIndex)
            tabs(TabControl1.SelectedIndex).listBox.Items.RemoveAt(recordIndex)
            If recordIndex >= tabs(TabControl1.SelectedIndex).listBox.Items.Count Then
                tabs(TabControl1.SelectedIndex).listBox.SelectedIndex = recordIndex - 1
            Else
                tabs(TabControl1.SelectedIndex).listBox.SelectedIndex = recordIndex
            End If
            ListBoxControlUpdate()
        End If
    End Sub

    Private Sub removeRecord(ByVal recordIndex As Integer)
        tabs(TabControl1.SelectedIndex).records.RemoveAt(recordIndex)
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
            If tabs(tabindex).records.Count = tabs(tabindex).recordMin Then
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

    Private Sub openEditForm(ByRef recordIndex As Integer, ByRef attrclasses As List(Of AttributeClass), ByRef task As String)
        Dim record As Record = tabs(TabControl1.SelectedIndex).records(recordIndex)
        Dim editForm As Form = New Form
        editForm.Text = task
        Dim panel1 As Panel = New Panel
        editForm.Size = New Drawing.Size(330, 325)
        panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        panel1.Location = New Drawing.Point(0, 0)
        panel1.Size = New Drawing.Size(315, 250)
        panel1.AutoScroll = True
        editForm.FormBorderStyle = FormBorderStyle.FixedDialog
        editForm.MaximizeBox = False
        editForm.MinimizeBox = False
        Dim handle As List(Of Object)
        handle = New List(Of Object)
        Dim yPos As Integer = 8
        For attrIndex As Integer = 0 To Record.attributes.Count - 1
            If attrclasses(attrIndex).hidden = False Then

                Dim yIncrement As Integer = 0
                Dim helpIncrement As Integer = 0

                Select Case attrclasses(attrIndex).type

                    Case AttributeType.attrString
                        Dim textBox As TextBox = New TextBox
                        textBox.Size = New Drawing.Size(130, 15)
                        textBox.Location = New Drawing.Point(116, yPos)
                        textBox.Text = record.attributes(attrIndex).value
                        panel1.Controls.Add(textBox)
                        handle.Add(textBox)

                        If attrclasses(attrIndex).editable = False Then textBox.Enabled = False

                    'todo string max length

                    Case AttributeType.attrTextFile
                        Dim textBox As TextBox = New TextBox
                        textBox.Size = New Drawing.Size(130, 80)
                        textBox.Multiline = True
                        textBox.WordWrap = False
                        textBox.ScrollBars = ScrollBars.Both
                        textBox.Location = New Drawing.Point(116, yPos)
                        textBox.Text = record.attributes(attrIndex).value
                        panel1.Controls.Add(textBox)
                        handle.Add(textBox)

                        Dim tooltip = New ToolTip
                        tooltip.ShowAlways = True
                        Dim expandButton As LoadDataButton = New LoadDataButton
                        expandButton.record = record
                        expandButton.attrIndex = attrIndex
                        expandButton.handle2 = textBox
                        expandButton.Size = New Drawing.Size(23, 22)
                        expandButton.Location = New Drawing.Point(90, yPos - 1)
                        expandButton.Image = imgExpand
                        AddHandler expandButton.Click, AddressOf showTextEditor
                        tooltip.SetToolTip(expandButton, "Expand")
                        panel1.Controls.Add(expandButton)

                        'increase size button enabled
                        If attrclasses(attrIndex).editable = False Then
                            textBox.Enabled = False
                            expandButton.Enabled = False
                        End If

                        yIncrement = 65
                        helpIncrement = 32

                    'todo string max length

                    Case AttributeType.attrInteger
                        Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                        numBox.Size = New Drawing.Size(130, 15)
                        numBox.Location = New Drawing.Point(116, yPos)
                        numBox.Maximum = attrclasses(attrIndex).maxValue
                        numBox.Minimum = attrclasses(attrIndex).minValue
                        numBox.Value = record.attributes(attrIndex).value
                        panel1.Controls.Add(numBox)
                        handle.Add(numBox)

                        If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                    Case AttributeType.attrTogglableInteger

                        Dim label1 As Label = New Label
                        label1.Text = "Default"
                        label1.Size = New Drawing.Size(45, 15)
                        label1.Location = New Drawing.Point(116, yPos + 1)
                        panel1.Controls.Add(label1)

                        Dim checkBox1 As MyCheckBox = New MyCheckBox()
                        checkBox1.Size = New Drawing.Size(15, 15)
                        checkBox1.Location = New Drawing.Point(161, yPos + 2)
                        panel1.Controls.Add(checkBox1)

                        Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                        numBox.Size = New Drawing.Size(130, 15)
                        numBox.Location = New Drawing.Point(116, yPos + 17)
                        numBox.Maximum = attrclasses(attrIndex).maxValue
                        numBox.Minimum = attrclasses(attrIndex).minValue

                        'set value to empty when box ticked - this way dont have to update when shots textbox changes

                        If record.attributes(attrIndex).value = 0 Then
                            checkBox1.Checked = True
                            numBox.Value = 0
                            numBox.Enabled = False
                        Else
                            checkBox1.Checked = False
                            numBox.Value = record.attributes(attrIndex).value
                            numBox.Enabled = True
                        End If

                        checkBox1.attrIndex = attrIndex
                        checkBox1.numBox = numBox
                        AddHandler checkBox1.CheckedChanged, AddressOf updateToggleableInteger

                        panel1.Controls.Add(numBox)
                        handle.Add(numBox)

                        If attrclasses(attrIndex).editable = False Then
                            numBox.Enabled = False
                            checkBox1.Enabled = False
                        End If

                        yIncrement = 20
                        helpIncrement = 7

                    Case AttributeType.attrDouble
                        Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                        numBox.Size = New Drawing.Size(130, 15)
                        numBox.Location = New Drawing.Point(116, yPos)
                        numBox.Maximum = attrclasses(attrIndex).maxValue
                        numBox.Minimum = attrclasses(attrIndex).minValue
                        numBox.Increment = 0.01D
                        numBox.DecimalPlaces = 2
                        numBox.Text = record.attributes(attrIndex).value
                        panel1.Controls.Add(numBox)
                        handle.Add(numBox)

                        If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                    Case AttributeType.attrIntBool
                        Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                        comboBox.Size = New Drawing.Size(130, 15)
                        comboBox.Location = New Drawing.Point(116, yPos)
                        comboBox.Text = record.attributes(attrIndex).value
                        comboBox.Items.Add("True")
                        comboBox.Items.Add("False")
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                        If record.attributes(attrIndex).value = True Then
                            comboBox.SelectedIndex = 0
                        Else
                            comboBox.SelectedIndex = 1
                        End If
                        panel1.Controls.Add(comboBox)
                        handle.Add(comboBox)

                        If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                    Case AttributeType.attrBoolean
                        Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                        comboBox.Size = New Drawing.Size(130, 15)
                        comboBox.Location = New Drawing.Point(116, yPos)
                        comboBox.Text = record.attributes(attrIndex).value
                        comboBox.Items.Add("True")
                        comboBox.Items.Add("False")
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                        If record.attributes(attrIndex).value = True Then
                            comboBox.SelectedIndex = 0
                        Else
                            comboBox.SelectedIndex = 1
                        End If
                        panel1.Controls.Add(comboBox)
                        handle.Add(comboBox)

                        If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                    Case AttributeType.attrFile

                        Dim textBox

                        If attrclasses(attrIndex).resName.Contains("&") Then
                            textBox = New TextBox
                            textBox.Enabled = False
                        Else
                            textBox = New UnscrollableComboBox
                            textBox.DropDownStyle = ComboBoxStyle.DropDownList
                            For index = 0 To attrclasses(attrIndex).dInd.Count - 1
                                textBox.Items.Add(attrclasses(attrIndex).dInd(index))
                            Next
                        End If

                        textBox.Text = record.attributes(attrIndex).value
                        textBox.Size = New Drawing.Size(108, 15)
                        textBox.Location = New Drawing.Point(116, yPos)
                        panel1.Controls.Add(textBox)
                        handle.Add(textBox)



                        Dim tooltip = New ToolTip
                        tooltip.ShowAlways = True
                        Dim button As LoadDataButton = New LoadDataButton
                        button.record = record
                        button.attrIndex = attrIndex
                        button.handle2 = textBox
                        button.Size = New Drawing.Size(23, 22)
                        button.Location = New Drawing.Point(224, yPos - 1)
                        button.Image = imgOpenFile
                        AddHandler button.Click, AddressOf OpenFileDialog
                        tooltip.SetToolTip(button, "Open " & attrclasses(attrIndex).ext.ToUpper & " File")
                        panel1.Controls.Add(button)

                        If attrclasses(attrIndex).editable = False Then
                            button.Enabled = False
                            textBox.enabled = False 'disable in case it's a combo box
                        End If

                End Select

                Dim label As New Label
                label.Size = New Drawing.Size(100, 15)
                label.Location = New Drawing.Point(8, yPos + helpIncrement)
                label.Text = attrclasses(attrIndex).displayName
                panel1.Controls.Add(label)

                Dim helptooltip = New ToolTip
                helptooltip.ShowAlways = True
                Dim helpButton As HelpButton = New HelpButton
                helpButton.Size = New Drawing.Size(22, 22)
                helpButton.Location = New Drawing.Point(258, yPos + 2 + helpIncrement)
                helpButton.Image = imgHelp
                helptooltip.SetToolTip(helpButton, "Help")
                helpButton.attrIndex = attrIndex
                AddHandler helpButton.Click, AddressOf showHelp
                panel1.Controls.Add(helpButton)

                yPos += yIncrement + 25
            Else
                handle.Add(Nothing)
            End If
        Next

        editForm.Controls.Add(panel1)

        Dim buttonOk As New FormButton
        buttonOk.Size = New Drawing.Size(60, 28)
        buttonOk.Location = New Drawing.Point(175, 254)
        buttonOk.Text = "Ok"
        buttonOk.result = False
        buttonOk.handle2 = handle
        buttonOk.form = editForm
        AddHandler buttonOk.Click, AddressOf editFormOk
        editForm.Controls.Add(buttonOk)

        Dim buttonCancel As New FormButton
        buttonCancel.Size = New Drawing.Size(60, 28)
        buttonCancel.Location = New Drawing.Point(235, 254)
        buttonCancel.Text = "Cancel"
        buttonCancel.form = editForm
        AddHandler buttonCancel.Click, AddressOf formCancel
        editForm.Controls.Add(buttonCancel)

        editForm.ShowDialog()

        If buttonOk.result = True Then
            For attrIndex As Integer = 0 To record.attributes.Count - 1
                If attrclasses(attrIndex).hidden = False Then
                    record.attributes(attrIndex).value = getHandleValue(attrclasses(attrIndex), handle(attrIndex))
                End If
            Next

            'update name in listbox
            Dim ri As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
            tabs(TabControl1.SelectedIndex).listBox.Items.RemoveAt(ri)
            tabs(TabControl1.SelectedIndex).listBox.Items.Insert(ri, tabs(TabControl1.SelectedIndex).getAttr(ri, "name"))
            tabs(TabControl1.SelectedIndex).listBox.SelectedIndex -= 1

        End If

    End Sub

    Function validateField(ByRef handle As Object)
        If handle.text.trim = "" Then Return False
        Return True
    End Function

    Function getHandleValue(ByRef attrClass As AttributeClass, ByRef handle As Object)
        Select Case attrClass.type
            Case AttributeType.attrString
                Return handle.text
            Case AttributeType.attrTextFile
                Return handle.text
            Case AttributeType.attrFile
                Return handle.text
            Case AttributeType.attrInteger
                If handle.Text = "" Then
                    Return 0
                Else
                    Return handle.value
                End If
            Case AttributeType.attrTogglableInteger
                If handle.Text = "" Then
                    Return 0
                Else
                    Return handle.value
                End If
            Case AttributeType.attrDouble
                If handle.Text = "" Then
                    Return 0
                Else
                    Return handle.value
                End If
            Case AttributeType.attrIntBool
                If handle.selectedIndex = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case AttributeType.attrBoolean
                If handle.selectedIndex = 0 Then
                    Return True
                Else
                    Return False
                End If
        End Select
        Return Nothing
    End Function

    Private Sub updateToggleableInteger(sender As Object, e As EventArgs)
        Dim tabIndex As Integer = TabControl1.SelectedIndex
        Dim recordIndex As Integer = tabs(tabIndex).listBox.SelectedIndex
        If sender.checked = True Then
            tabs(tabIndex).records(recordIndex).attributes(sender.attrIndex).value = 0
            sender.numBox.Text = ""
            sender.numbox.enabled = False
        Else
            Dim val As Integer = tabs(tabIndex).attributeClasses(sender.attrIndex).defaultValue
            tabs(tabIndex).records(recordIndex).attributes(sender.attrIndex).value = val
            sender.numBox.text = val
            sender.numbox.enabled = True
        End If

    End Sub

    Private Sub showTextEditor(sender As Object, e As EventArgs)

        Dim tabIndex As Integer = TabControl1.SelectedIndex
        Dim recordIndex As Integer = tabs(tabIndex).listBox.SelectedIndex
        Dim attrIndex As Integer = sender.attrIndex

        Dim textForm As Form = New Form
        textForm.Text = "Edit " & tabs(tabIndex).attributeClasses(attrIndex).displayName 'todo and name
        textForm.FormBorderStyle = FormBorderStyle.FixedDialog
        textForm.MaximizeBox = False
        textForm.MinimizeBox = False
        textForm.Size = New Drawing.Size(390, 346)

        Dim textBox As TextBox = New TextBox
        textBox.Size = New Drawing.Size(350, 250)
        textBox.Multiline = True
        textBox.WordWrap = False
        'textBox.ScrollBars = ScrollBars.Both
        textBox.Location = New Drawing.Point(10, 10)
        textBox.Text = sender.handle2.text
        textForm.Controls.Add(textBox)

        Dim buttonOk As New FormButton
        buttonOk.Size = New Drawing.Size(60, 28)
        buttonOk.Location = New Drawing.Point(230, 270)
        buttonOk.Text = "Ok"
        buttonOk.result = False
        buttonOk.form = textForm
        AddHandler buttonOk.Click, AddressOf formOk
        textForm.Controls.Add(buttonOk)

        Dim buttonCancel As New FormButton
        buttonCancel.Size = New Drawing.Size(60, 28)
        buttonCancel.Location = New Drawing.Point(300, 270)
        buttonCancel.Text = "Cancel"
        buttonCancel.form = textForm
        AddHandler buttonCancel.Click, AddressOf formCancel
        textForm.Controls.Add(buttonCancel)

        textForm.ShowDialog()

        If buttonOk.result = True Then
            sender.handle2.text = textBox.Text
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
        If attrClass.resName.Contains("&") Then
            nameLabel.Text = attrClass.displayName & ":"
        Else
            nameLabel.Text = attrClass.displayName & " (" & attrClass.resName & ")" & ":"
        End If
        nameLabel.Size = New Drawing.Size(190, 14)
        nameLabel.Location = New Drawing.Point(10, 5)
        helpForm.Controls.Add(nameLabel)
        Dim label As Label = New Label
        label.Size = New Drawing.Size(170, 150)
        label.MaximumSize = New Drawing.Size(170, 150)
        label.Location = New Drawing.Point(10, 25)
        label.AutoSize = True
        label.Text = attrClass.helpInfo
        helpForm.Controls.Add(label)
        helpForm.Size = New Drawing.Size(200, label.Size.Height + 80)
        helpForm.ShowDialog()
    End Sub

    Private Sub OpenFileDialog(sender As Object, e As EventArgs)
        Dim dialog As New OpenFileDialog()
        Dim ext As String = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrindex).ext
        Dim resName As String = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrindex).resName
        dialog.InitialDirectory = tempdir
        dialog.Filter = ext & " files (*." & ext & ")|*." & ext
        dialog.FilterIndex = 0
        dialog.RestoreDirectory = True  'what does this do again??
        If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            'check if file exists in temp or game folder - if not auto rename
            Dim filename As String = System.IO.Path.GetFileName(dialog.FileName).ToLower
            If OpenFileChecker(resName, filename) = True Then
                Dim fileno As Integer = 1
                Do
                    fileno += 1
                    filename = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName) & " (" & fileno & ")." & ext
                Loop Until Not OpenFileChecker(resName, filename)
            End If

            'update data
            tabs(TabControl1.SelectedIndex).getAttrClass(resName).dind.add(filename.ToLower)
            tabs(TabControl1.SelectedIndex).getAttrClass(resName).data.add(My.Computer.FileSystem.ReadAllBytes(System.IO.Path.GetFullPath(dialog.FileName)))

            'copy file to temp folder
            'FileSystem.FileCopy(System.IO.Path.GetFullPath(dialog.FileName), tempFolder & filename)

            'update textbox
            If resName.Contains("&") Then
                sender.handle2.text = filename
            Else
                sender.handle2.items.add(filename.ToLower)
                sender.handle2.selectedindex = sender.handle2.items.count - 1
            End If


            tempdir = System.IO.Path.GetFullPath(dialog.FileName)

        End If
    End Sub

    Private Function OpenFileChecker(ByVal resName As String, ByVal filename As String)
        'If My.Computer.FileSystem.FileExists(tempFolder & filename) Then Return True
        Dim attrIndex = tabs(TabControl1.SelectedIndex).getAttrIndex(resName)
        For index = 0 To tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).dInd.Count - 1
            If tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).dInd(index).ToLower = filename.ToLower Then Return True
        Next
        'If My.Computer.FileSystem.FileExists(dir & gamefolder & filename) Then Return True
        Return False
    End Function

    Friend Class MyCheckBox
        Inherits CheckBox

        Public attrIndex As Integer
        Public numBox As NumericUpDown

    End Class

    Friend Class HelpButton
        Inherits PictureBox

        Public attrIndex As Integer

    End Class

    Friend Class FormButton
        Inherits Button

        Public handle2 As List(Of Object)

        Public form As Form
        Public result As Boolean

    End Class

    Friend Class LoadDataButton
        Inherits Button

        Public record As Record
        Public attrIndex As Integer
        Public handle2

    End Class

    Friend Class UnscrollableComboBox
        Inherits ComboBox

        Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
            Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
            mwe.Handled = True
        End Sub
    End Class

    Friend Class UnscrollableNumericUpDown
        Inherits NumericUpDown

        Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
            Dim mwe As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
            mwe.Handled = True
        End Sub
    End Class


    Private Sub formOk(sender As Object, e As EventArgs)
        sender.result = True
        sender.form.Close()
    End Sub

    Private Sub editFormOk(sender As Object, e As EventArgs)
        sender.result = True
        For index = 0 To sender.handle2.count - 1
            If tabs(TabControl1.SelectedIndex).attributeClasses(index).validation = True Then
                If Not validateField(sender.handle2(index)) Then
                    sender.result = False
                    mess(tabs(TabControl1.SelectedIndex).attributeClasses(index).displayName & " invalid")
                    Return
                End If
            End If
        Next
        sender.form.Close()
    End Sub

    Private Sub formCancel(sender As Object, e As EventArgs)
        sender.form.Close()
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

        version(0) = "0.1.0"
        lastSupportedVersion = 0


        printLog("Mod Manager Version: " & managerVersion & " last supported version " & version(lastSupportedVersion))

        Me.Text = "Carnivores Mod Manager v" & managerVersion

        If My.Computer.FileSystem.FileExists(dir & "\MODDAT\VINFO") Then
            Dim vinfo = My.Computer.FileSystem.ReadAllBytes(dir & "\MODDAT\VINFO")
            modVersion = vinfo(0)
            If modVersion > lastSupportedVersion Then
                DoHalt("This version of Modders Edition is not supported. Please install the lastest version of Mod Manager.")
            Else
                If modVersion < lastSupportedVersion Then
                    'TODO IN FUTURE MOD MANAGER VERSIONS - OFFER TO UPGRADE COPY OF MODDERS EDITION TO LATEST SUPPORTED VERSION, else run in old mode!!!
                End If
                printLog("Mod Version: " & version(modVersion))
            End If
        Else
            DoHalt("Modders Edition is not installed on this copy of Carnivores")
        End If

        tempdir = dir

        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(0, "name", "Camouflage")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(1, "name", "Radar")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(2, "name", "Cover Scent")
        tabs(EquipmentTab).addRecord()
        tabs(EquipmentTab).setAttr(3, "name", "Double Ammo")

        'add area records
        Dim mapInd As Integer = tabs(AreaTab).getAttrIndex("map&")
        Dim mapTemp As Integer
        Do
            mapTemp = tabs(AreaTab).records.Count
            If My.Computer.FileSystem.FileExists(dir & tabs(AreaTab).attributeClasses(mapInd).gameFolder & tabs(AreaTab).records.Count + 1 & "." & tabs(AreaTab).attributeClasses(mapInd).ext) Then
                tabs(AreaTab).addRecord()
            End If
        Loop Until tabs(AreaTab).records.Count = mapTemp

        readRes()

        'read non-res files
        For tabIndex As Integer = 0 To 7
            For recordIndex As Integer = 0 To tabs(tabIndex).records.Count - 1
                For attrIndex As Integer = 0 To tabs(tabIndex).attributeClasses.Count - 1

                    If tabs(tabIndex).attributeClasses(attrIndex).resName.Contains("&") Then

                        If tabs(tabIndex).attributeClasses(attrIndex).type = AttributeType.attrTextFile Then

                            tabs(tabIndex).records(recordIndex).attributes(attrIndex).value = ""
                            Dim areaType As Boolean = (tabIndex = AreaTab) 'area names are stored in menu text
                            FileOpen(128, dir & tabs(tabIndex).attributeClasses(attrIndex).gameFolder & recordIndex + 1 & "." & tabs(tabIndex).attributeClasses(attrIndex).ext, OpenMode.Input)
                            While Not EOF(128)
                                If areaType = True Then
                                    tabs(AreaTab).setAttr(recordIndex, "name", LineInput(128)) 'temp
                                    LineInput(128)
                                    areaType = False
                                End If
                                tabs(tabIndex).records(recordIndex).attributes(attrIndex).value &= LineInput(128)
                                tabs(tabIndex).records(recordIndex).attributes(attrIndex).value &= vbCrLf
                            End While
                            FileClose(128)

                        Else


                            Dim str As String = tabs(tabIndex).attributeClasses(attrIndex).gameFolder
                            Dim pos As Integer = str.LastIndexOf("\") + 1
                            tabs(tabIndex).records(recordIndex).attributes(attrIndex).value = str.Substring(pos, str.Length - pos) & recordIndex + 1 & "." & tabs(tabIndex).attributeClasses(attrIndex).ext
                            tabs(tabIndex).attributeClasses(attrIndex).dInd.Add(tabs(tabIndex).records(recordIndex).attributes(attrIndex).value)
                            tabs(tabIndex).attributeClasses(attrIndex).data.Add(My.Computer.FileSystem.ReadAllBytes(dir & tabs(tabIndex).attributeClasses(attrIndex).gameFolder & recordIndex + 1 & "." & tabs(tabIndex).attributeClasses(attrIndex).ext))

                        End If

                    End If

                Next
            Next
        Next



    End Sub

    Private Sub printLog(ByVal txt As String)
        PrintLine(2, txt)
        Console.WriteLine(txt)
    End Sub

    Private Sub mess(ByVal _mess As String)
        MsgBox(_mess, vbOKOnly, "Error")
    End Sub

    Function conf(ByVal _mess As String)
        Return MsgBox(_mess, vbYesNo, "Confirmation") = DialogResult.Yes
    End Function

    Private Sub DoHalt(ByVal _mess As String)
        mess(_mess)
        shutDown()
    End Sub

    Private Sub readRes()
        'this should be reshunt unless you fix up rexhunters menu
        'gotta print to res and reshunt
        If Not My.Computer.FileSystem.FileExists(dir & "\HUNTDAT\_RES.TXT") Then
            DoHalt("_RES.TXT not found")
        End If
        FileOpen(1, dir & "\HUNTDAT\_RES.TXT", OpenMode.Input)

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
                        Dim attrIndex = tabs(tabIndex).getAttrIndex(resName)
                        Dim value
                        Select Case tabs(tabIndex).attributeClasses(attrIndex).type
                            Case AttributeType.attrString
                                value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                            Case AttributeType.attrFile
                                value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                            Case AttributeType.attrInteger
                                value = Trim(line.Substring(line.IndexOf("=") + 1))
                            Case AttributeType.attrTogglableInteger
                                value = Trim(line.Substring(line.IndexOf("=") + 1))
                            Case AttributeType.attrDouble
                                value = Trim(line.Substring(line.IndexOf("=") + 1))
                            Case AttributeType.attrBoolean
                                If line.Contains("TRUE") Then value = True Else value = False
                            Case AttributeType.attrIntBool
                                If Trim(line.Substring(line.IndexOf("=") + 1)) > 0 Then value = True Else value = False
                        End Select
                        Dim recordIndex = tabs(tabIndex).records.Count - 1

                        If tabs(tabIndex).attributeClasses(attrIndex).type = AttributeType.attrFile Then
                            Dim newFile As Boolean = True
                            For index = 0 To tabs(tabIndex).attributeClasses(attrIndex).dInd.Count - 1
                                If tabs(tabIndex).attributeClasses(attrIndex).dInd(index) = value Then
                                    newFile = False
                                End If
                            Next
                            If newFile = True Then
                                tabs(tabIndex).attributeClasses(attrIndex).dInd.Add(value)
                                tabs(tabIndex).attributeClasses(attrIndex).data.Add(My.Computer.FileSystem.ReadAllBytes(dir & tabs(tabIndex).getAttrClass(resName).gameFolder & value))
                            End If
                        End If
                        tabs(tabIndex).setAttr(recordIndex, resName, value)
                    End If
                    line = LineInput(1)
                Loop Until line.Contains("}")
                If debug Then
                    printLog("READ " & tabs(tabIndex).nameS & " : " & tabs(tabIndex).records.Count - 1)
                    For atrIndex As Integer = 0 To tabs(tabIndex).attributeClasses.Count - 1
                        printLog(tabs(tabIndex).attributeClasses(atrIndex).displayName & "=" & tabs(tabIndex).records(tabs(tabIndex).records.Count - 1).attributes(atrIndex).value)
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
                tabs(AreaTab).setAttr(a, "price", Trim(line.Substring(line.IndexOf("=") + 1)))
                a += 1
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
                    printLog("EQUIPMENT" & i & "=" & tabs(EquipmentTab).getAttr(i, "price"))
                Next
            End If
            printLog("---------------------------------------------------------------------")
            End If

    End Sub


    Public Class Tab
        Public name, nameS As String
        'Public addImage, editImage, removeImage As string
        Public addButton, removeButton, editButton As Button
        Public addToolTip, removeToolTip, editToolTip As ToolTip
        Public listBox As ListBox
        Public recordLock As Boolean ' prevent adding/deleting records

        Public attributeClasses As List(Of AttributeClass)
        Public recordMax, recordMin As Integer
        Public records As List(Of Record)

        Public Sub New()
            records = New List(Of Record)
            attributeClasses = New List(Of AttributeClass)
        End Sub

        Public Sub addAttribute(ByVal name As String, ByVal res As String, ByVal type As AttributeType, ByVal defaultValue As Object,
                                ByVal min As Integer, ByVal max As Integer, ByVal hide As Boolean, ByVal gameFolder As String,
                                ByVal ext As String, ByVal edit As Boolean, ByVal valida As Boolean, ByVal help As String)
            attributeClasses.Add(New AttributeClass(name, res, type, defaultValue, min, max, hide, gameFolder, ext, edit, valida, help))
            'clear temp folder
            'If type = AttributeType.attrFile Then
            'Dim path As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Remove(0, 6) & "\" & tempFolder & "\"
            'For Each fi In New IO.DirectoryInfo(path).GetFiles("*." & ext)
            'My.Computer.FileSystem.DeleteFile(path & fi.Name)
            'Next
            'End If
        End Sub

        Public Sub addRecord()
            records.Add(New Record)
            records(records.Count - 1).attributes = New List(Of Attribute)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                records(records.Count - 1).attributes.Add(New Attribute(attributeClasses(attrIndex).defaultValue))
            Next
        End Sub

        Public Sub setAttr(ByVal recordIndex As Integer, ByVal resName As String, ByVal _value As Object)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    records(recordIndex).attributes(attrIndex).value = _value
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

        Public Function getAttrIndex(ByVal resName As String)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    Return attrIndex
                End If
            Next
            Return -1
        End Function

        Public Function getAttrClass(resName As String)
            For attrIndex As Integer = 0 To attributeClasses.Count - 1
                If attributeClasses(attrIndex).resName = resName Then
                    Return attributeClasses(attrIndex)
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
        Public validation As Boolean

        Public gameFolder, ext As String 'file data only

        Public helpInfo As String

        Public data As List(Of Byte())
        Public dInd As List(Of String)

        Public Sub New(ByVal _name As String, ByVal _res As String, ByVal _type As AttributeType, ByVal defVal As Object,
                       ByVal min As Integer, ByVal max As Integer, ByVal _hidden As Boolean, ByVal _gameFolder As String,
                       ByVal _ext As String, ByVal _edit As Boolean, ByVal _validation As Boolean, ByVal help As String)
            displayName = _name
            resName = _res
            type = _type
            defaultValue = defVal
            minValue = min
            maxValue = max
            hidden = _hidden
            gameFolder = _gameFolder
            ext = _ext
            editable = _edit
            validation = _validation
            helpInfo = help

            If type = AttributeType.attrFile Then
                data = New List(Of Byte())
                dInd = New List(Of String)
            End If
        End Sub

    End Class

    Enum AttributeType
        attrString  'basic text
        attrTextFile
        attrInteger 'whole numbers
        attrTogglableInteger 'whole numbers
        attrDouble  'decimal numbers
        attrBoolean ' true/false
        attrIntBool ' true/false - written with numbers in the res

        attrFile
    End Enum

    Public Class Attribute
        Public value As Object
        'Public data() As Byte

        Public Sub New(ByVal _value As Object)
            value = _value
        End Sub
        Public Sub setvalue(ByVal _value As Object)
            value = _value
        End Sub
    End Class

End Class
