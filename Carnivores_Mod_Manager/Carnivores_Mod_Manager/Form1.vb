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

    Dim imgAdd As Bitmap = New Bitmap(My.Resources.add, New Size(32, 32))
    Dim imgAddSub As Bitmap = New Bitmap(My.Resources.add, New Size(20, 20))
    Dim imgEdit As Bitmap = New Bitmap(My.Resources.edit, New Size(32, 32))
    Dim imgEditSub As Bitmap = New Bitmap(My.Resources.edit, New Size(20, 20))
    Dim imgDelete As Bitmap = New Bitmap(My.Resources.delete, New Size(32, 32))
    Dim imgMinusSub As Bitmap = New Bitmap(My.Resources.minus, New Size(20, 20))
    Dim imgUp As Bitmap = New Bitmap(My.Resources.up, New Size(32, 32))
    Dim imgDown As Bitmap = New Bitmap(My.Resources.down, New Size(32, 32))
    Dim imgExpand As Bitmap = New Bitmap(My.Resources.expand, New Size(20, 20))
    Dim imgOpenFile As Bitmap = New Bitmap(My.Resources.openfile, New Size(20, 20))
    Dim imgHelp As Bitmap = New Bitmap(My.Resources.help, New Size(19, 19))

    Dim aiList As List(Of AI)

    Dim nameSpeed As List(Of String)

    Dim editForm As Form
    Dim panel1 As Panel
    Dim handle As List(Of Object)
    Dim panel As List(Of Panel)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'todo load dialog?
        init()
        setupTabData()
        loadData()
        drawTabs()

    End Sub

    Private Sub init()
        FileOpen(2, "MODMANAGER.LOG", OpenMode.Output)
        printLog("")
        debug = False
        startMoney = 0

        Dim commandArgs As String() = Environment.GetCommandLineArgs()

        If commandArgs.Length < 1 Then DoHalt("No Carnivores folder selected")

        For argIndex As Integer = 0 To commandArgs.Length - 1
            If commandArgs(argIndex).Contains("dir=") Then dir = commandArgs(argIndex).Substring(4)
            If commandArgs(argIndex) = "-debug" Then debug = True
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


    End Sub

    Private Sub setupTabData()

        nameSpeed = New List(Of String)
        nameSpeed.Add("runspd")     '0
        nameSpeed.Add("wlkspd")     '1
        nameSpeed.Add("flyspd")     '2
        nameSpeed.Add("gldspd")     '3
        nameSpeed.Add("swmspd")     '4

        aiList = New List(Of AI)
        aiList.Add(New AI("Moshops", 1, 4, nameSpeed.Count))                    '0
        aiList.Add(New AI("Galimimus", 2, 5, nameSpeed.Count))                  '1
        aiList.Add(New AI("Dimorphodon", 3, 4, nameSpeed.Count))                '2
        aiList.Add(New AI("Pteranodon", 4, 4, nameSpeed.Count))                 '3
        aiList.Add(New AI("Dimetrodon", 5, 4, nameSpeed.Count))                 '4
        aiList.Add(New AI("Parasaurolophus", 10, 0, nameSpeed.Count))           '5
        aiList.Add(New AI("Pachycephalosaurus", 19, 0, nameSpeed.Count))        '6
        aiList.Add(New AI("Ankylosaurus", 11, 0, nameSpeed.Count))              '7
        aiList.Add(New AI("Stegosaurus", 12, 0, nameSpeed.Count))               '8
        aiList.Add(New AI("Allosaurus", 13, 0, nameSpeed.Count))                '9
        aiList.Add(New AI("Chasmosaurus", 14, 0, nameSpeed.Count))              '10
        aiList.Add(New AI("Velociraptor", 15, 0, nameSpeed.Count))              '11
        aiList.Add(New AI("Spinosaurus", 16, 0, nameSpeed.Count))               '12
        aiList.Add(New AI("Ceratosaurus", 17, 0, nameSpeed.Count))              '13
        aiList.Add(New AI("T-Rex", 18, 0, nameSpeed.Count))                     '14
        aiList.Add(New AI("Brachiosaurus", -1, 0, nameSpeed.Count))             '15
        aiList.Add(New AI("Dangerous Brachiosaurus", -5, 0, nameSpeed.Count))   '16
        aiList.Add(New AI("Land Brachiosaurus", -6, 0, nameSpeed.Count))        '17
        aiList.Add(New AI("Ichthyornis", -2, 0, nameSpeed.Count))               '18
        aiList.Add(New AI("Fish", -3, 0, nameSpeed.Count))                      '19

        For index = 0 To 19
            aiList(index).active = New List(Of String)
        Next

        For Each index In {0, 1, 4}
            aiList(index).active.Add("runAnim")
            aiList(index).active.Add("walkAnim")
            aiList(index).active.Add("runspd")
            aiList(index).active.Add("wlkspd")

            aiList(index).active.Add("idleAnim")

            aiList(index).active.Add("canswim")
            aiList(index).active.Add("swimAnim")
            aiList(index).active.Add("swmspd")
            aiList(index).active.Add("waterLevel")

            aiList(index).active.Add("fearCall")
        Next

        aiList(1).active.Add("slideAnim")

        For Each index In {2, 3}
            aiList(index).active.Add("flyAnim")
            aiList(index).active.Add("glideAnim")
            aiList(index).active.Add("flyspd")
            aiList(index).active.Add("gldspd")
        Next

        'MOSH
        aiList(0).defaultSpeed(0) = 0.6 'run
        aiList(0).defaultSpeed(1) = 0.3 'wlk
        aiList(0).defaultSpeed(4) = 0.3 'swm

        'GALL
        aiList(1).defaultSpeed(0) = 0.9 'run
        aiList(1).defaultSpeed(1) = 0.32 'wlk
        aiList(1).defaultSpeed(4) = 0.32 'swm

        'DIMOR
        aiList(2).defaultSpeed(2) = 1.5 'fly
        aiList(2).defaultSpeed(3) = 1.3 'gld

        'PTERA
        aiList(3).defaultSpeed(2) = 1.5 'fly
        aiList(3).defaultSpeed(3) = 1.3 'gld

        'DIMET
        aiList(4).defaultSpeed(0) = 0.6 'run
        aiList(4).defaultSpeed(1) = 0.3 'wlk
        aiList(4).defaultSpeed(4) = 0.3 'swm





        'TODO THIS WILL ALL BE VERSION-SPECIFIC IN FUTURE

        tabs(AreaTab) = New Tab()
        tabs(AreaTab).name = "Areas"
        tabs(AreaTab).nameS = "Area"
        tabs(AreaTab).recordMax = 10
        tabs(AreaTab).recordMin = 0
        tabs(AreaTab).recordLock = False
        tabs(AreaTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the area in the hunt menu.")
        tabs(AreaTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\AREA", "txt", True, False, False, False, "The description for the area in the hunt menu.")
        tabs(AreaTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\AREA", "tga", True, True, False, False, "The image for the area in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(AreaTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, False, False, "The cost of hunting in the area.")
        tabs(AreaTab).addAttribute("Map File", "map&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\AREAS\AREA", "map", True, True, False, False, "The file that stores the terrain information, texture positions, object positions, sound positions, water and fog information.")
        tabs(AreaTab).addAttribute("Resource File", "rsc&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\AREAS\AREA", "rsc", True, True, False, False, "The file that stores textures, objects, animations, hitbox information and sounds for the map.")

        tabs(HuntableTab) = New Tab()
        tabs(HuntableTab).name = "Huntable Creatures"
        tabs(HuntableTab).nameS = "Huntable Creature"
        tabs(HuntableTab).recordMax = 10
        tabs(HuntableTab).recordMin = 0
        tabs(HuntableTab).recordLock = False
        tabs(HuntableTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the creature in the hunt menu.")
        tabs(HuntableTab).addAttribute("Description (Metric)", "desm&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\DINO", "txm", True, False, False, False, "The description for the creature in the hunt menu when the user has set measurements to 'metric' in options.")
        tabs(HuntableTab).addAttribute("Description (US)", "desu&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\DINO", "txu", True, False, False, False, "The description for the creature in the hunt menu the user has set measurements to 'US' in options.")
        tabs(HuntableTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\DINO", "tga", True, True, False, False, "The image for the creature in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(HuntableTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, False, False, "The cost of hunting the creature.")
        Dim carFileClass As AttributeClass = New AttributeClass("CAR File", "file", AttributeType.attrCar, "", 0, 509, False, "\HUNTDAT\", "car", True, True, False, False, "The file that stores the creature model, texture, animations and sound effects.")
        tabs(HuntableTab).addAttribute(carFileClass)
        'tabs(HuntableTab).addAttribute("Call Icon", "callimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\CALL", "tga", True, True, "The image displayed in the top right when a call is selected during a hunt. Must be saved as a 16 bit uncompressed TGA.")
        'TODO - CALL FILE NAME FORMATS NEED TO ADJUSTED in C:ME!!!!

        tabs(AmbientTab) = New Tab()
        tabs(AmbientTab).name = "Ambient Creatures"
        tabs(AmbientTab).nameS = "Ambient Creature"
        tabs(AmbientTab).recordMax = 5
        tabs(AmbientTab).recordLock = False
        tabs(AmbientTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the creature in the binoculars.")
        tabs(AmbientTab).addAttribute("Slot", "ai", AttributeType.attrSlot, 1, 1, 5, True, "", "", True, False, False, False, "")
        'remember to change slot minmax
        tabs(AmbientTab).addAttribute("AI", "clone", AttributeType.attrAI, 1, 0, 4, False, "", "", True, False, False, False, "The AI used by the creature")
        'remember to change ai minmax 'default is AI number, min/max is listAi index
        tabs(AmbientTab).addAttribute(carFileClass)

        tabs(AmbientTab).addAttribute("Health", "health", AttributeType.attrInteger, 0, 0, 2147483647, False, "", "", True, False, False, False, "The creatures health points. Each hit will subtract the weapons power from the creatures health. Note that if the CAR file contains mortal zones, it will be possible to one-hit kill.")

        tabs(AmbientTab).addAttribute("Run Animation", "runAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, False, "The running animation for the creature - animations can be viewed by opening the car file in C3Dit.")
        tabs(AmbientTab).addAttribute("Run Speed", "runspd", AttributeType.attrSpd, 0D, -1000D, 1000D, False, "", "", True, False, True, False, "The movement speed of the creature when running.")

        tabs(AmbientTab).addAttribute("Walk Animation", "walkAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, False, "The walking animation for the creature - animations can be viewed by opening the car file in C3Dit.")
        tabs(AmbientTab).addAttribute("Walk Speed", "wlkspd", AttributeType.attrSpd, 0D, -1000D, 1000D, False, "", "", True, False, True, False, "The movement speed of the creature when walking.")

        tabs(AmbientTab).addAttribute("Slide Animation", "slideAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, False, "The sliding animation for the creature - animations can be viewed by opening the car file in C3Dit.")

        tabs(AmbientTab).addAttribute("Fly Animation", "flyAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, False, "The flying animation for the creature - animations can be viewed by opening the car file in C3Dit.")
        tabs(AmbientTab).addAttribute("Fly Speed", "flyspd", AttributeType.attrSpd, 0D, -1000D, 1000D, False, "", "", True, False, True, False, "The movement speed of the creature when flying.")

        tabs(AmbientTab).addAttribute("Glide Animation", "glideAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, False, "The gliding animation for the creature - animations can be viewed by opening the car file in C3Dit.")
        tabs(AmbientTab).addAttribute("Glide Speed", "gldspd", AttributeType.attrSpd, 0D, -1000D, 1000D, False, "", "", True, False, True, False, "The movement speed of the creature when gliding.")

        tabs(AmbientTab).addAttribute("Idle Animations", "idleAnim", AttributeType.attrAnimMulti, -1, 0, 32, False, "", "", True, True, True, False, "The idle animations for the creature. Each creatures can have between 0 and 32 idle animations. Animations can be viewed by opening the car file in C3Dit.")

        tabs(AmbientTab).addAttribute("Minimum Size (%)", "scale0", AttributeType.attrScale, 1000, 0, 2147483647, False, "", "", True, False, False, False, "Minimum size of the creature. Variation will adjust model size, mass, length and movement speed.")
        tabs(AmbientTab).addAttribute("Maximum Size (%)", "scaleA", AttributeType.attrScaleA, 0, 0, 2147483647, False, "", "", True, True, False, False, "Maximum size of the creature. Variation will adjust model size, mass, length and movement speed.")

        tabs(AmbientTab).addAttribute("Can Swim", "canswim", AttributeType.attrSwimmer, False, 0, 0, False, "", "", True, False, True, False, "When set to true, the creature will be able to swim.")
        tabs(AmbientTab).addAttribute("Swim Animation", "swimAnim", AttributeType.attrAnim, -1, 0, 32, False, "", "", True, True, True, True, "The swimming animation for the creature - animations can be viewed by opening the car file in C3Dit. Only applicable if Can Swim is set to true.")
        tabs(AmbientTab).addAttribute("Swim Speed", "swmspd", AttributeType.attrSpd, 0D, -1000D, 1000D, False, "", "", True, False, True, True, "The movement speed of the creature when swimming. Only applicable if Can Swim is set to true.")
        tabs(AmbientTab).addAttribute("Water Level", "waterLevel", AttributeType.attrInteger, 0, 0, 2147483647, False, "", "", True, False, True, True, "Controls how far a creature can wade into the water before having to swim. Only applicable if Can Swim is set to true.")

        tabs(AmbientTab).addAttribute("Afraid of Call", "fearCall", AttributeType.attrFearCall, 0, 0, 64, False, "", "", True, True, True, False, "These calls can be used to scare the creature away.")



        'Average Scale scale0

        'Scale Variation scaleA
        'change mass desc once these two are done

        tabs(AmbientTab).addAttribute("Mass", "mass", AttributeType.attrDouble, 0D, 0D, 10D, False, "", "", True, False, False, False, "The creatures mass displayed in the binoculars. The mass of each individual creature can vary depending on Minimum and Maximum Size.")






        tabs(AmbientCorpseTab) = New Tab()
        tabs(AmbientCorpseTab).name = "Ambient Corpses"
        tabs(AmbientCorpseTab).nameS = "Ambient Corpse"
        tabs(AmbientCorpseTab).recordMax = 4
        tabs(AmbientCorpseTab).recordLock = False
        tabs(AmbientCorpseTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the creature in the binoculars.")
        tabs(AmbientCorpseTab).addAttribute(carFileClass)
        'NOTE ALL AMBIENT CORPSES MUST HAVE AI 6,7,OR 8 and matching CLONE - hidden?


        'Leave waterD out for now?? Water die Is Not part of deathType ???

        tabs(MapAmbientTab) = New Tab()
        tabs(MapAmbientTab).name = "Map Ambient Creatures"
        tabs(MapAmbientTab).nameS = "Map Ambient Creature"
        tabs(MapAmbientTab).recordMax = 1024
        tabs(MapAmbientTab).recordLock = False
        tabs(MapAmbientTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the creature in the binoculars.")
        tabs(MapAmbientTab).addAttribute(carFileClass)

        tabs(WeaponTab) = New Tab()
        tabs(WeaponTab).name = "Weapons"
        tabs(WeaponTab).nameS = "Weapon"
        tabs(WeaponTab).recordMax = 10
        tabs(WeaponTab).recordMin = 0
        tabs(WeaponTab).recordLock = False
        tabs(WeaponTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", True, True, False, False, "The display name for the weapon in the menu.")
        tabs(WeaponTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\WEAPON", "txt", True, False, False, False, "The description for the weapon in the hunt menu.")
        tabs(WeaponTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\WEAPON", "tga", True, True, False, False, "The image for the weapon in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(WeaponTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, False, False, "The cost of using the weapon on a hunt.")
        tabs(WeaponTab).addAttribute("CAR File", "file", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\WEAPONS\", "car", True, True, False, False, "The file that stores the weapon model, texture, animations and sound effects.")
        tabs(WeaponTab).addAttribute("Bullet Image", "pic", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\WEAPONS\", "tga", True, True, False, False, "The bullet image used to display remaining ammo. Must be saved as a 16 bit uncompressed TGA.")
        'tabs(WeaponTab).addAttribute("Gunshot Sound", "gunshot", AttributeType.attrFile, "", 0, 509, True, "\MULTIPLAYER\GUNSHOTS\", "wav", True, False, False, False, "The gunshot sound effect which other players can hear from a distance in multiplayer. Must be saved as a 22050 Hz rate mono WAV.")
        tabs(WeaponTab).addAttribute("Ammo Count", "shots", AttributeType.attrInteger, 1, 1, 2147483647, False, "", "", True, False, False, False, "The amount of ammo the hunter takes for this weapon on a hunt (Will be doubled if the hunter selects double ammo in equipment).")
        tabs(WeaponTab).addAttribute("Magazine Capacity", "reload", AttributeType.attrTogglableInteger, 0, 0, 2147483647, False, "", "", True, False, False, False, "The maximum number of rounds that can be fired before reloading. If set to default, the magazine capacity will equal the ammo count.") 'togglableint - if off, value is 0
        tabs(WeaponTab).addAttribute("Projectile Count", "trace", AttributeType.attrInteger, 1, 1, 2147483647, False, "", "", True, False, False, False, "The number of projectiles fired with each shot.")
        tabs(WeaponTab).addAttribute("Fire Power", "power", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, False, False, "The damage inflicted by the weapon. Note: the vanilla menu can only display values between 0 and 8.")
        tabs(WeaponTab).addAttribute("Accuracy", "prec", AttributeType.attrDouble, 0D, -10D, 2D, False, "", "", True, False, False, False, "Average bullet precision - 2.00 gives 100% accuracy. Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Volume", "loud", AttributeType.attrDouble, 0D, -10D, 10D, False, "", "", True, False, False, False, "The maximum distance creatures can hear gunshots (depending on wind direction). Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Rate of Fire", "rate", AttributeType.attrDouble, 0D, -10D, 10D, False, "", "", True, False, False, False, "The rate of fire for the weapon - this value only effects the menu stats and doesn't effect gameplay. Note: the vanilla menu can only display values between 0.00 and 2.00.")
        tabs(WeaponTab).addAttribute("Scope Zoom", "optic", AttributeType.attrIntBool, False, 0, 0, False, "", "", True, False, False, False, "When set to true, the hunter's screen will zoom in when the weapon is equipped.")
        tabs(WeaponTab).addAttribute("Bullet Fall", "fall", AttributeType.attrIntBool, False, 0, 0, False, "", "", True, False, False, False, "When set to true, projectile trajectory will drop over time.")

        tabs(EquipmentTab) = New Tab()
        tabs(EquipmentTab).name = "Equipment"
        tabs(EquipmentTab).nameS = "Equipment"
        tabs(EquipmentTab).recordMax = 4
        tabs(EquipmentTab).recordMin = 0
        tabs(EquipmentTab).recordLock = True
        tabs(EquipmentTab).addAttribute("Name", "name", AttributeType.attrString, "", 0, 509, False, "", "", False, True, False, False, "The display name for the equipment item in the menu.")
        tabs(EquipmentTab).addAttribute("Description", "desc&", AttributeType.attrTextFile, "", 0, 509, False, "\HUNTDAT\MENU\TXT\EQUIP", "nfo", True, False, False, False, "The description for the equipment item in the hunt menu.")
        tabs(EquipmentTab).addAttribute("Menu Image", "menuimg&", AttributeType.attrFile, "", 0, 509, False, "\HUNTDAT\MENU\PICS\EQUIP", "tga", True, True, False, False, "The image for the equipment item in the hunt menu. Must be saved as a 16 bit uncompressed TGA.")
        tabs(EquipmentTab).addAttribute("Price", "price", AttributeType.attrInteger, 0, -2147483648, 2147483647, False, "", "", True, False, False, False, "The display name for the equipment item in the menu.")

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

                tabs(tabIndex).upToolTip = New ToolTip
                tabs(tabIndex).upToolTip.ShowAlways = True
                tabs(tabIndex).upButton = New Button
                tabs(tabIndex).upButton.Image = imgUp
                tabs(tabIndex).upButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).upButton.Location = New Drawing.Point(366, 146)
                tabs(tabIndex).upButton.Enabled = False
                AddHandler tabs(tabIndex).upButton.Click, AddressOf upRecord
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).upButton)

                tabs(tabIndex).downToolTip = New ToolTip
                tabs(tabIndex).downToolTip.ShowAlways = True
                tabs(tabIndex).downButton = New Button
                tabs(tabIndex).downButton.Image = imgDown
                tabs(tabIndex).downButton.Size = New Drawing.Size(38, 38)
                tabs(tabIndex).downButton.Location = New Drawing.Point(366, 192)
                tabs(tabIndex).downButton.Enabled = False
                AddHandler tabs(tabIndex).downButton.Click, AddressOf downRecord
                TabControl1.TabPages(tabIndex).Controls.Add(tabs(tabIndex).downButton)

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

    Private Sub upRecord()
        Dim index As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
        Dim record As Record = tabs(TabControl1.SelectedIndex).records(index)
        tabs(TabControl1.SelectedIndex).records(index) = tabs(TabControl1.SelectedIndex).records(index - 1)
        tabs(TabControl1.SelectedIndex).records(index - 1) = record
        Dim name As String = tabs(TabControl1.SelectedIndex).listBox.Items(index).ToString
        tabs(TabControl1.SelectedIndex).listBox.Items(index) = tabs(TabControl1.SelectedIndex).listBox.Items(index - 1)
        tabs(TabControl1.SelectedIndex).listBox.Items(index - 1) = name
        tabs(TabControl1.SelectedIndex).listBox.SelectedIndex -= 1
        ListBoxControlUpdate()
    End Sub

    Private Sub downRecord()
        Dim index As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
        Dim record As Record = tabs(TabControl1.SelectedIndex).records(index)
        tabs(TabControl1.SelectedIndex).records(index) = tabs(TabControl1.SelectedIndex).records(index + 1)
        tabs(TabControl1.SelectedIndex).records(index + 1) = record
        Dim name As String = tabs(TabControl1.SelectedIndex).listBox.Items(index).ToString
        tabs(TabControl1.SelectedIndex).listBox.Items(index) = tabs(TabControl1.SelectedIndex).listBox.Items(index + 1)
        tabs(TabControl1.SelectedIndex).listBox.Items(index + 1) = name
        tabs(TabControl1.SelectedIndex).listBox.SelectedIndex += 1
        ListBoxControlUpdate()

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
        Dim ri As Integer = tabs(TabControl1.SelectedIndex).listBox.SelectedIndex
        tabs(TabControl1.SelectedIndex).listBox.Items(ri) = tabs(TabControl1.SelectedIndex).getAttr(ri, "name")
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
            tabs(tabindex).upButton.Enabled = False
            tabs(tabindex).downButton.Enabled = False
            tabs(tabindex).addButton.Visible = False
            tabs(tabindex).editButton.Visible = True
            tabs(tabindex).removeButton.Visible = False
            tabs(tabindex).upButton.Visible = False
            tabs(tabindex).downButton.Visible = False
            tabs(tabindex).editToolTip.SetToolTip(tabs(tabindex).editButton, "Edit " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
        Else
            If tabs(tabindex).records.Count = tabs(tabindex).recordMax Then
                tabs(tabindex).addButton.Enabled = False
            Else
                tabs(tabindex).addButton.Enabled = True
            End If
            If tabs(tabindex).records.Count = tabs(tabindex).recordMin Then
                tabs(tabindex).removeButton.Enabled = False
            Else
                tabs(tabindex).removeToolTip.SetToolTip(tabs(tabindex).removeButton, "Delete " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
                tabs(tabindex).removeButton.Enabled = True
            End If
            If tabs(tabindex).records.Count = 0 Then
                tabs(tabindex).editButton.Enabled = False
                tabs(tabindex).upButton.Enabled = False
                tabs(tabindex).downButton.Enabled = False
            Else
                tabs(tabindex).editButton.Enabled = True
                tabs(tabindex).editToolTip.SetToolTip(tabs(tabindex).editButton, "Edit " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name"))
                If tabs(tabindex).listBox.SelectedIndex = 0 Then
                    tabs(tabindex).upButton.Enabled = False
                    tabs(tabindex).upToolTip.SetToolTip(tabs(tabindex).upButton, "Move " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name") & " up")
                Else
                    tabs(tabindex).upButton.Enabled = True
                    tabs(tabindex).upToolTip.SetToolTip(tabs(tabindex).upButton, "Move " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name") & " up")
                End If
                If tabs(tabindex).listBox.SelectedIndex = tabs(tabindex).listBox.Items.Count - 1 Then
                    tabs(tabindex).downButton.Enabled = False
                    tabs(tabindex).downToolTip.SetToolTip(tabs(tabindex).downButton, "Move " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name") & " down")
                Else
                    tabs(tabindex).downButton.Enabled = True
                    tabs(tabindex).downToolTip.SetToolTip(tabs(tabindex).downButton, "Move " & tabs(tabindex).getAttr(tabs(tabindex).listBox.SelectedIndex, "name") & " down")
                End If
            End If
        End If
    End Sub

    Function findIndex(ByVal o As String, ByVal l As List(Of String))
        For i As Integer = 0 To l.Count - 1
            If o = l(i) Then Return i
        Next
    End Function

    Function getAI(ByVal ai As Integer)
        For index = 0 To aiList.Count - 1
            If aiList(index).id = ai Then Return aiList(index)
        Next
        Return 0 'if not, return hunter
    End Function

    Function getAIbyName(ByVal n As String)
        For index = 0 To aiList.Count - 1
            If aiList(index).name = n Then Return index
        Next
        Return 0 'If Not, Return hunter
    End Function

    Function getSpdPos(ByVal resname As String)
        For index = 0 To nameSpeed.Count - 1
            If nameSpeed(index) = resname Then Return index
        Next
        Return -1
    End Function

    Function getAIIndex(ByVal ai As Integer)
        For index = 0 To aiList.Count - 1
            If aiList(index).id = ai Then Return index
        Next
    End Function

    Function getAI(ByVal n As String)
        For index = 0 To aiList.Count - 1
            If aiList(index).name = n Then Return index
        Next
    End Function

    Private Sub openEditForm(ByRef recordIndex As Integer, ByRef attrclasses As List(Of AttributeClass), ByRef task As String)
        Dim record As Record = tabs(TabControl1.SelectedIndex).records(recordIndex)
        editForm = New Form
        editForm.Text = task
        panel1 = New Panel
        editForm.Size = New Drawing.Size(330, 325)
        panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        panel1.Location = New Drawing.Point(0, 0)
        panel1.Size = New Drawing.Size(315, 250)
        panel1.AutoScroll = True
        editForm.FormBorderStyle = FormBorderStyle.FixedDialog
        editForm.MaximizeBox = False
        editForm.MinimizeBox = False

        handle = New List(Of Object)
        panel = New List(Of Panel)

        Dim yPos As Integer = 8
        For attrIndex As Integer = 0 To record.attributes.Count - 1

            Dim active = True
            If attrclasses(attrIndex).hidden = True Then
                active = False
            ElseIf attrclasses(attrIndex).AIdependant = True Then
                If Not aiList(getAIIndex(tabs(TabControl1.SelectedIndex).getAttr(recordIndex, "clone"))).active.Contains(attrclasses(attrIndex).resName) Then
                    active = False
                End If
            End If

            'If active = True Then

            Dim yIncrement As Integer = 0
            Dim helpIncrement As Integer = 0

            panel.Add(New Panel)
            panel(panel.Count - 1).Location = New Drawing.Point(8, yPos)

            Select Case attrclasses(attrIndex).type

                Case AttributeType.attrSlot
                    handle.Add(Nothing)

                Case AttributeType.attrString
                    Dim textBox As TextBox = New TextBox
                    textBox.Size = New Drawing.Size(130, 15)
                    textBox.Location = New Drawing.Point(116, 0)
                    textBox.Text = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(textBox)
                    handle.Add(textBox)

                    If attrclasses(attrIndex).editable = False Then textBox.Enabled = False

                    'todo string max length

                Case AttributeType.attrAI
                    Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                    comboBox.Size = New Drawing.Size(130, 15)
                    comboBox.Location = New Drawing.Point(116, 0)
                    For index = attrclasses(attrIndex).minValue To attrclasses(attrIndex).maxValue
                        comboBox.Items.Add(aiList(index).name)
                    Next
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                    comboBox.SelectedIndex = getAIIndex(record.attributes(attrIndex).value)
                    panel(panel.Count - 1).Controls.Add(comboBox)

                    AddHandler comboBox.SelectedIndexChanged, AddressOf changeAI


                    handle.Add(comboBox)

                    If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                Case AttributeType.attrAnimMulti

                    Dim pane As Panel = New Panel
                    handle.Add(pane)

                    pane.Size = New Drawing.Size(130, 80)
                    pane.Location = New Drawing.Point(116, 0)


                    pane.AutoScroll = True
                    pane.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D

                    panel(panel.Count - 1).Controls.Add(pane)

                    Dim addT = New ToolTip
                    addT.ShowAlways = True
                    Dim addB As DGVButton = New DGVButton
                    addB.attrIndex = attrIndex
                    addB.record = record
                    addB.recordIndex = recordIndex
                    addB.Size = New Drawing.Size(23, 23)
                    addB.Location = New Drawing.Point(93, -1)
                    addB.Image = imgAddSub
                    AddHandler addB.Click, AddressOf addMultiAnim
                    addT.SetToolTip(addB, "Add Animation")
                    panel(panel.Count - 1).Controls.Add(addB)

                    Dim removeT = New ToolTip
                    removeT.ShowAlways = True
                    Dim removeB As DGVButton = New DGVButton
                    removeB.attrIndex = attrIndex
                    removeB.recordIndex = recordIndex
                    removeB.Size = New Drawing.Size(23, 23)
                    removeB.Location = New Drawing.Point(93, 21)
                    removeB.Image = imgMinusSub
                    AddHandler removeB.Click, AddressOf removeDGV
                    removeT.SetToolTip(removeB, "Remove Animation")
                    panel(panel.Count - 1).Controls.Add(removeB)

                    If attrclasses(attrIndex).editable = False Then
                        addB.Enabled = False
                        removeB.Enabled = False
                    End If

                    For i As Integer = 0 To record.attributes(attrIndex).value.count - 1
                        addMultiAnim2(attrIndex, recordIndex, record.attributes(attrIndex).value(i))
                    Next

                    updateDGVButtons(attrIndex) ' in case of 0 reocrds

                    yIncrement = 65
                    helpIncrement = 32

                Case AttributeType.attrFearCall

                    Dim pane As Panel = New Panel
                    handle.Add(pane)

                    pane.Size = New Drawing.Size(130, 80)
                    pane.Location = New Drawing.Point(116, 0)


                    pane.AutoScroll = True
                    pane.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D

                    panel(panel.Count - 1).Controls.Add(pane)

                    Dim addT = New ToolTip
                    addT.ShowAlways = True
                    Dim addB As DGVButton = New DGVButton
                    addB.attrIndex = attrIndex
                    addB.recordIndex = recordIndex
                    addB.Size = New Drawing.Size(23, 23)
                    addB.Location = New Drawing.Point(93, -1)
                    addB.Image = imgAddSub
                    AddHandler addB.Click, AddressOf addFearCall
                    addT.SetToolTip(addB, "Add Call")
                    panel(panel.Count - 1).Controls.Add(addB)

                    Dim removeT = New ToolTip
                    removeT.ShowAlways = True
                    Dim removeB As DGVButton = New DGVButton
                    removeB.attrIndex = attrIndex
                    removeB.recordIndex = recordIndex
                    removeB.Size = New Drawing.Size(23, 23)
                    removeB.Location = New Drawing.Point(93, 21)
                    removeB.Image = imgMinusSub
                    AddHandler removeB.Click, AddressOf removeDGV
                    removeT.SetToolTip(removeB, "Remove Call")
                    panel(panel.Count - 1).Controls.Add(removeB)

                    If attrclasses(attrIndex).editable = False Then
                        addB.Enabled = False
                        removeB.Enabled = False
                    End If

                    For i As Integer = 0 To record.attributes(attrIndex).value.count - 1
                        addFearCall2(attrIndex, aiList(getAIIndex(record.attributes(attrIndex).value(i))).name)
                    Next

                    updateDGVButtons(attrIndex) ' in case of 0 reocrds

                    yIncrement = 65
                    helpIncrement = 32


                Case AttributeType.attrTextFile
                    Dim textBox As TextBox = New TextBox
                    textBox.Size = New Drawing.Size(130, 80)
                    textBox.Multiline = True
                    textBox.WordWrap = False
                    textBox.ScrollBars = ScrollBars.Both
                    textBox.Location = New Drawing.Point(116, 0)
                    textBox.Text = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(textBox)
                    handle.Add(textBox)

                    Dim tooltip = New ToolTip
                    tooltip.ShowAlways = True
                    Dim expandButton As LoadDataButton = New LoadDataButton
                    expandButton.record = record
                    expandButton.attrIndex = attrIndex
                    expandButton.handle2 = textBox
                    expandButton.Size = New Drawing.Size(23, 23)
                    expandButton.Location = New Drawing.Point(90, -1)
                    expandButton.Image = imgExpand
                    AddHandler expandButton.Click, AddressOf showTextEditor
                    tooltip.SetToolTip(expandButton, "Expand")
                    panel(panel.Count - 1).Controls.Add(expandButton)

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
                    numBox.Location = New Drawing.Point(116, 0)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue
                    numBox.Value = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(numBox)
                    handle.Add(numBox)

                    If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                Case AttributeType.attrAnim

                    Dim comboBox As UnscrollableAnimComboBox = New UnscrollableAnimComboBox 'custom class overrides scroll wheeling through options
                    comboBox.Size = New Drawing.Size(130, 15)
                    comboBox.Location = New Drawing.Point(116, 0)
                    For Each animName In record.animList
                        comboBox.Items.Add(animName)
                    Next

                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                    comboBox.SelectedIndex = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(comboBox)
                    comboBox.prevValue = comboBox.SelectedIndex
                    comboBox.senderAttr = attrIndex
                    comboBox.recordIndex = recordIndex
                    AddHandler comboBox.SelectedIndexChanged, AddressOf animReorder
                    handle.Add(comboBox)
                    If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False


                Case AttributeType.attrTogglableInteger

                    Dim label1 As Label = New Label
                    label1.Text = "Default"
                    label1.Size = New Drawing.Size(45, 15)
                    label1.Location = New Drawing.Point(116, 1)
                    panel(panel.Count - 1).Controls.Add(label1)

                    Dim checkBox1 As MyCheckBox = New MyCheckBox()
                    checkBox1.Size = New Drawing.Size(15, 15)
                    checkBox1.Location = New Drawing.Point(161, 2)
                    panel(panel.Count - 1).Controls.Add(checkBox1)

                    Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                    numBox.Size = New Drawing.Size(130, 15)
                    numBox.Location = New Drawing.Point(116, 17)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue

                    'set value to empty when box ticked - this way dont have to update when shots textbox changes

                    If record.attributes(attrIndex).value = attrclasses(attrIndex).defaultValue Then
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

                    panel(panel.Count - 1).Controls.Add(numBox)
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
                    numBox.Location = New Drawing.Point(116, 0)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue
                    numBox.Increment = 0.01D
                    numBox.DecimalPlaces = 2
                    numBox.Text = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(numBox)
                    handle.Add(numBox)

                    If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                Case AttributeType.attrScale
                    Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                    numBox.Size = New Drawing.Size(130, 15)
                    numBox.Location = New Drawing.Point(116, 0)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue
                    numBox.Increment = 0.1D
                    numBox.DecimalPlaces = 1
                    numBox.Text = record.attributes(attrIndex).value / 10
                    panel(panel.Count - 1).Controls.Add(numBox)
                    handle.Add(numBox)

                    If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                Case AttributeType.attrScaleA
                    Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                    numBox.Size = New Drawing.Size(130, 15)
                    numBox.Location = New Drawing.Point(116, 0)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue
                    numBox.Increment = 0.1D
                    numBox.DecimalPlaces = 1
                    numBox.Text = (record.attributes(attrIndex).value + tabs(TabControl1.SelectedIndex).getAttr(recordIndex, "scale0")) / 10
                    panel(panel.Count - 1).Controls.Add(numBox)
                    handle.Add(numBox)

                    If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                Case AttributeType.attrSpd
                    Dim numBox As UnscrollableNumericUpDown = New UnscrollableNumericUpDown 'custom class overrides scroll wheeling through options
                    numBox.Size = New Drawing.Size(130, 15)
                    numBox.Location = New Drawing.Point(116, 0)
                    numBox.Maximum = attrclasses(attrIndex).maxValue
                    numBox.Minimum = attrclasses(attrIndex).minValue
                    numBox.Increment = 0.001D
                    numBox.DecimalPlaces = 3
                    numBox.Text = record.attributes(attrIndex).value
                    panel(panel.Count - 1).Controls.Add(numBox)
                    handle.Add(numBox)

                    If attrclasses(attrIndex).editable = False Then numBox.Enabled = False

                Case AttributeType.attrIntBool
                    Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                    comboBox.Size = New Drawing.Size(130, 15)
                    comboBox.Location = New Drawing.Point(116, 0)
                    comboBox.Text = record.attributes(attrIndex).value
                    comboBox.Items.Add("True")
                    comboBox.Items.Add("False")
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                    If record.attributes(attrIndex).value = True Then
                        comboBox.SelectedIndex = 0
                    Else
                        comboBox.SelectedIndex = 1
                    End If
                    panel(panel.Count - 1).Controls.Add(comboBox)
                    handle.Add(comboBox)

                    If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                Case AttributeType.attrBoolean
                    Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                    comboBox.Size = New Drawing.Size(130, 15)
                    comboBox.Location = New Drawing.Point(116, 0)
                    comboBox.Text = record.attributes(attrIndex).value
                    comboBox.Items.Add("True")
                    comboBox.Items.Add("False")
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                    If record.attributes(attrIndex).value = True Then
                        comboBox.SelectedIndex = 0
                    Else
                        comboBox.SelectedIndex = 1
                    End If
                    panel(panel.Count - 1).Controls.Add(comboBox)
                    handle.Add(comboBox)

                    If attrclasses(attrIndex).editable = False Then comboBox.Enabled = False

                Case AttributeType.attrSwimmer
                    Dim comboBox As UnscrollableComboBox = New UnscrollableComboBox 'custom class overrides scroll wheeling through options
                    comboBox.Size = New Drawing.Size(130, 15)
                    comboBox.Location = New Drawing.Point(116, 0)
                    comboBox.Text = record.attributes(attrIndex).value
                    comboBox.Items.Add("True")
                    comboBox.Items.Add("False")
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList
                    If record.attributes(attrIndex).value = True Then
                        comboBox.SelectedIndex = 0
                    Else
                        comboBox.SelectedIndex = 1
                    End If
                    AddHandler comboBox.SelectedIndexChanged, AddressOf swimChange
                    panel(panel.Count - 1).Controls.Add(comboBox)
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
                    textBox.Location = New Drawing.Point(116, 0)
                    panel(panel.Count - 1).Controls.Add(textBox)
                    handle.Add(textBox)



                    Dim tooltip = New ToolTip
                    tooltip.ShowAlways = True
                    Dim button As LoadDataButton = New LoadDataButton
                    button.record = record
                    button.attrIndex = attrIndex
                    button.handle2 = textBox
                    button.Size = New Drawing.Size(23, 23)
                    button.Location = New Drawing.Point(224, -1)
                    button.Image = imgOpenFile
                    AddHandler button.Click, AddressOf OpenFileDialog
                    tooltip.SetToolTip(button, "Open " & attrclasses(attrIndex).ext.ToUpper & " File")
                    panel(panel.Count - 1).Controls.Add(button)

                    If attrclasses(attrIndex).editable = False Then
                        button.Enabled = False
                        textBox.enabled = False 'disable in case it's a combo box
                    End If

                Case AttributeType.attrCar

                    Dim textBox As UnscrollableCarComboBox


                    textBox = New UnscrollableCarComboBox
                    textBox.DropDownStyle = ComboBoxStyle.DropDownList
                    For index = 0 To attrclasses(attrIndex).dInd.Count - 1
                        textBox.Items.Add(attrclasses(attrIndex).dInd(index))
                    Next

                    textBox.Text = record.attributes(attrIndex).value
                    textBox.Size = New Drawing.Size(108, 15)
                    textBox.Location = New Drawing.Point(116, 0)
                    panel(panel.Count - 1).Controls.Add(textBox)
                    handle.Add(textBox)



                    Dim tooltip = New ToolTip
                    tooltip.ShowAlways = True
                    Dim button As LoadDataButton = New LoadDataButton
                    button.record = record
                    button.attrIndex = attrIndex
                    button.handle2 = textBox
                    textBox.record = record
                    textBox.attrIndex = attrIndex
                    button.Size = New Drawing.Size(23, 23)
                    button.Location = New Drawing.Point(224, -1)
                    button.Image = imgOpenFile
                    AddHandler button.Click, AddressOf OpenFileDialog
                    tooltip.SetToolTip(button, "Open " & attrclasses(attrIndex).ext.ToUpper & " File")
                    panel(panel.Count - 1).Controls.Add(button)

                    If attrclasses(attrIndex).editable = False Then
                        button.Enabled = False
                        textBox.enabled = False 'disable in case it's a combo box
                    End If

                    textBox.lastIndex = textBox.SelectedIndex
                    AddHandler textBox.SelectedIndexChanged, AddressOf checkCarValid

            End Select

            panel(panel.Count - 1).Size = New Drawing.Size(300, 25 + yIncrement)

            Dim label As New Label
            label.Size = New Drawing.Size(100, 15)
            label.Location = New Drawing.Point(8, helpIncrement)
            label.Text = attrclasses(attrIndex).displayName
            panel(panel.Count - 1).Controls.Add(label)

            Dim helptooltip = New ToolTip
            helptooltip.ShowAlways = True
            Dim helpButton As HelpButton = New HelpButton
            helpButton.Size = New Drawing.Size(22, 22)
            helpButton.Location = New Drawing.Point(258, 2 + helpIncrement)
            helpButton.Image = imgHelp
            helptooltip.SetToolTip(helpButton, "Help")
            helpButton.attrIndex = attrIndex
            AddHandler helpButton.Click, AddressOf showHelp
            panel(panel.Count - 1).Controls.Add(helpButton)

            panel1.Controls.Add(panel(panel.Count - 1))

            If active = True Then
                yPos += yIncrement + 25
                panel(panel.Count - 1).Visible = True
            Else
                panel(panel.Count - 1).Visible = False
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

        For i2 As Integer = 0 To tabs(TabControl1.SelectedIndex).attributeClasses.Count - 1
            If tabs(TabControl1.SelectedIndex).attributeClasses(i2).type = AttributeType.attrSwimmer Then
                If handle(i2).selectedIndex = 1 Then
                    swimChange2(handle(i2))
                End If
            End If
        Next

        editForm.ShowDialog()

        If buttonOk.result = True Then
            For attrIndex As Integer = 0 To record.attributes.Count - 1
                Dim active = True
                If attrclasses(attrIndex).AIdependant = True Then
                    If Not aiList(getAI(handle(tabs(TabControl1.SelectedIndex).getAttrIndex("clone")).text)).active.Contains(attrclasses(attrIndex).resName) Then
                        active = False
                    End If
                End If

                If active = True Then
                    If attrclasses(attrIndex).hidden = False Then
                        record.attributes(attrIndex).value = getHandleValue(attrclasses(attrIndex), handle(attrIndex), record)
                    End If
                Else
                    record.attributes(attrIndex).value = attrclasses(attrIndex).defaultValue
                End If
            Next
        End If

    End Sub

    Sub addFearCall(sender As Object, e As EventArgs)
        addFearCall2(sender.attrindex, "")
    End Sub

    Sub addMultiAnim(sender As Object, e As EventArgs)
        addMultiAnim2(sender.attrindex, sender.recordIndex, -1)
    End Sub

    Sub removeDGV(sender As Object, e As EventArgs)
        'handle(sender.attrIndex).Rows.RemoveAt(handle(sender.attrIndex).rows.count - 1)
        handle(sender.attrIndex).controls.RemoveAt(handle(sender.attrIndex).controls.Count - 1)
        updateDGVButtons(sender.attrIndex)
    End Sub

    Sub updateDGVButtons(ByVal attrIndex As Integer)
        If handle(attrIndex).controls.count < tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).maxValue Then
            panel(attrIndex).Controls(1).Enabled = True
        Else
            panel(attrIndex).Controls(1).Enabled = False
        End If

        If handle(attrIndex).controls.count > tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).minValue Then
            panel(attrIndex).Controls(2).Enabled = True
        Else
            panel(attrIndex).Controls(2).Enabled = False
        End If
    End Sub

    Sub addFearCall2(ByVal attrIndex As Integer, ByVal val As String)
        Dim cb = New UnscrollableAnimComboBox
        cb.Size = New Drawing.Size(100, 15)
        cb.Location = New Drawing.Point(5, handle(attrIndex).controls.count * 25 + 5 + handle(attrIndex).AutoScrollPosition.Y)
        For index = 0 To tabs(HuntableTab).records.Count - 1
            cb.Items.Add(tabs(HuntableTab).getAttr(index, "name"))
        Next
        cb.DropDownStyle = ComboBoxStyle.DropDownList
        cb.prevValue = cb.SelectedIndex
        cb.senderAttr = attrIndex
        cb.Text = val
        If tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).editable = False Then cb.Enabled = False
        handle(attrIndex).controls.Add(cb)
        updateDGVButtons(attrIndex)
    End Sub

    Sub addMultiAnim2(ByVal attrIndex As Integer, ByVal recordIndex As Integer, ByVal si As Integer)
        Dim record As Record = tabs(TabControl1.SelectedIndex).records(recordIndex)
        Dim cb = New UnscrollableAnimComboBox
        cb.Size = New Drawing.Size(100, 15)
        cb.Location = New Drawing.Point(5, handle(attrIndex).controls.count * 25 + 5 + handle(attrIndex).AutoScrollPosition.Y)
        For Each animName In record.animList
            cb.Items.Add(animName)
        Next
        cb.DropDownStyle = ComboBoxStyle.DropDownList
        cb.prevValue = cb.SelectedIndex
        cb.senderAttr = attrIndex
        cb.senderCtrl = handle(attrIndex).controls.count
        cb.recordIndex = recordIndex
        cb.SelectedIndex = si
        AddHandler cb.SelectedIndexChanged, AddressOf animReorder
        If tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).editable = False Then cb.Enabled = False
        handle(attrIndex).controls.Add(cb)
        updateDGVButtons(attrIndex)
    End Sub

    Sub swimChange2(ByVal h As Object)
        For attrIndex = 0 To handle.Count - 1
            If tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).swimmer = True Then
                If h.selectedindex = 0 Then
                    handle(attrIndex).enabled = True
                Else
                    handle(attrIndex).enabled = False

                End If
            End If
        Next
    End Sub

    Sub swimChange(sender As Object, e As EventArgs)
        swimChange2(sender)
    End Sub

    Sub drawEditFormAttributes(sender As Object)
        Dim attrclasses As List(Of AttributeClass) = tabs(TabControl1.SelectedIndex).attributeClasses
        Dim yPos As Integer = 8
        For attrIndex = 0 To panel.Count - 1

            panel(attrIndex).Location = New Drawing.Point(8, yPos)

            Dim active = True
            If attrclasses(attrIndex).hidden = True Then
                active = False
            ElseIf attrclasses(attrIndex).AIdependant = True Then
                If Not aiList(getAI(sender.text)).active.Contains(attrclasses(attrIndex).resName) Then
                    active = False
                End If
            End If

            Dim yIncrement As Integer = 0
            If attrclasses(attrIndex).type = AttributeType.attrTextFile Then yIncrement = 65
            If attrclasses(attrIndex).type = AttributeType.attrFearCall Then yIncrement = 65
            If attrclasses(attrIndex).type = AttributeType.attrAnimMulti Then yIncrement = 65
            If attrclasses(attrIndex).type = AttributeType.attrTogglableInteger Then yIncrement = 20

            If active = True Then
                yPos += yIncrement + 25
                panel(attrIndex).Visible = True
            Else
                panel(attrIndex).Visible = False
                'handle(attrIndex).value = attrclasses(attrIndex).defaultValue
            End If


        Next
        editForm.Refresh()
    End Sub

    'Private Sub dgvAnimReorder(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs)
    'Dim cb As ComboBox = TryCast(e.Control, ComboBox)
    'If cb IsNot Nothing Then
    '
    'Dim editingComboBox As ComboBox = DirectCast(e.Control, ComboBox)
    'RemoveHandler() editingComboBox.SelectedIndexChanged,
    '            New EventHandler(AddressOf animReorderDGV)
    'AddHandler() editingComboBox.SelectedIndexChanged,
    '            New EventHandler(AddressOf animReorderDGV)
    'End If
    'End Sub

    Sub animReorder(sender As Object, e As EventArgs)

        Dim multi As Boolean = False
        Dim newValue As Integer
        If tabs(TabControl1.SelectedIndex).attributeClasses(sender.senderAttr).type = AttributeType.attrAnim Then
            newValue = handle(sender.senderAttr).selectedIndex
        ElseIf tabs(TabControl1.SelectedIndex).attributeClasses(sender.senderAttr).type = AttributeType.attrAnimMulti Then
            newValue = handle(sender.senderAttr).controls(sender.senderctrl).selectedIndex
            multi = True
        End If

        If sender.progChange = True Then
            sender.progChange = False
        Else

            For attrIndex = 0 To tabs(TabControl1.SelectedIndex).attributeClasses.Count - 1
                If tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).type = AttributeType.attrAnim And attrIndex <> sender.senderAttr Then
                    If handle(attrIndex).selectedIndex = newValue Then
                        handle(attrIndex).progChange = True
                        handle(attrIndex).selectedIndex = -1
                    End If
                ElseIf tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex).type = AttributeType.attrAnimMulti Then

                    For controlIndex = 0 To handle(attrIndex).controls.count - 1

                        If handle(attrIndex).controls(controlIndex).selectedIndex = newValue And (controlIndex <> sender.senderCtrl Or multi = False) Then
                            handle(attrIndex).controls(controlIndex).progChange = True
                            handle(attrIndex).controls(controlIndex).selectedIndex = -1
                        End If

                    Next

                End If
            Next
        End If
        sender.prevValue = newValue
    End Sub

    Function validateField(ByRef handle3 As Object, ByVal attrType As AttributeType)
        If attrType = AttributeType.attrAnimMulti Or attrType = AttributeType.attrFearCall Then
            For rowNo = 0 To handle3.controls.count - 1
                If handle3.controls(rowNo).selectedindex < 0 Then Return False
            Next
        ElseIf attrType = AttributeType.attrscaleA Then
            If handle3.value < handle(tabs(TabControl1.SelectedIndex).getAttrIndex("scale0")).value Then Return False
        Else
            If handle3.text.trim = "" Then Return False
        End If
        Return True
    End Function



    Function getHandleValue(ByRef attrClass As AttributeClass, ByRef handle3 As Object, ByVal record As Record)
        Select Case attrClass.type
            Case AttributeType.attrString
                Return handle3.text
            Case AttributeType.attrAI
                Return aiList(getAI(handle3.text)).id
            Case AttributeType.attrFearCall
                Dim l = New List(Of Integer)
                For rowNo = 0 To handle3.controls.count - 1
                    l.Add(aiList(getAIbyName(handle3.controls(rowNo).text)).id)
                Next
                Return l
            Case AttributeType.attrAnimMulti
                Dim l = New List(Of Integer)
                For rowNo = 0 To handle3.controls.count - 1
                    l.Add(findIndex(handle3.controls(rowNo).text, record.animList))
                Next
                Return l
            Case AttributeType.attrTextFile
                Return handle3.text
            Case AttributeType.attrFile
                Return handle3.text
            Case AttributeType.attrCar
                Return handle3.text
            Case AttributeType.attrAnim
                Return handle3.SelectedIndex
            Case AttributeType.attrInteger
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return handle3.value
                End If
            Case AttributeType.attrTogglableInteger
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return handle3.value
                End If
            Case AttributeType.attrDouble
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return handle3.value
                End If
            Case AttributeType.attrScale
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return handle3.value * 10
                End If
            Case AttributeType.attrScaleA
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return (handle3.value - handle(tabs(TabControl1.SelectedIndex).getAttrIndex("scale0")).value) * 10
                End If
            Case AttributeType.attrSpd
                If handle3.Text = "" Then
                    Return 0
                Else
                    Return handle3.value
                End If
            Case AttributeType.attrIntBool
                If handle3.selectedIndex = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case AttributeType.attrBoolean
                If handle3.selectedIndex = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case AttributeType.attrSwimmer
                If handle3.selectedIndex = 0 Then
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
        sender.numBox.text = tabs(tabIndex).attributeClasses(sender.attrIndex).defaultValue
        If sender.checked = True Then
            sender.numbox.enabled = False
        Else
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

    Private Sub changeAI(sender As Object, e As EventArgs)
        Dim r As Record = tabs(TabControl1.SelectedIndex).records(tabs(TabControl1.SelectedIndex).listBox.SelectedIndex)
        'conf("Are you sure you want to change AI?.")Dim h As Object = handle(tabs(TabControl1.SelectedIndex).getAttrIndex("file"))
        Dim h As Object = handle(tabs(TabControl1.SelectedIndex).getAttrIndex("file"))
        If r.animList.Count < aiList(getAI(sender.text)).minAnim And h.text <> "" Then
            mess(h.text & " has too few animations to be used on " & aiList(getAI(sender.text)).name & " AI (at least " & aiList(getAI(sender.text)).minAnim & " animations needed)")
            h.progChange = True
            'r.animList = New List(Of String)
            For attrIndex As Integer = 0 To handle.Count - 1
                Dim aCl = tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex)
                If aCl.type = AttributeType.attrAnim Then
                    handle(attrIndex).items.clear
                ElseIf aCl.type = AttributeType.attrAnimmulti Then
                    For Each cb In handle(attrIndex).controls
                        cb.items.clear
                    Next
                End If
            Next
            h.selectedindex = -1
        Else
            Dim resetSpd As Boolean = conf("Reset speeds to defaults for " & aiList(getAI(sender.text)).name & " AI?")
            For attrIndex As Integer = 0 To handle.Count - 1
                Dim aCl = tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex)
                If aCl.type = AttributeType.attrAnim Then
                    handle(attrIndex).selectedIndex = -1
                ElseIf aCl.type = AttributeType.attrAnimMulti Then
                    For Each cb In handle(attrIndex).controls
                        cb.selectedIndex = -1
                    Next
                ElseIf aCl.type = AttributeType.attrSpd And resetspd Then
                    handle(attrIndex).value = aiList(getAI(sender.text)).defaultSpeed(getSpdPos(aCl.resName))
                End If
            Next
        End If
        drawEditFormAttributes(sender)

    End Sub

    Private Sub checkCarValid(sender As Object, e As EventArgs)
        If sender.progChange = True Then
            sender.progChange = False
        Else

            Dim animList As List(Of String)
            Dim attrClass = tabs(TabControl1.SelectedIndex).attributeClasses(sender.attrIndex)
            Dim ai As AI = aiList(getAI(handle(tabs(TabControl1.SelectedIndex).getAttrIndex("clone")).text))
            animList = readAnimations(attrClass.data(sender.selectedIndex))
            If animList.Count < ai.minAnim Then
                mess(ai.name & " AI requires a CAR file with at least " & ai.minAnim & " Animations.")
                sender.progChange = True
                sender.selectedIndex = sender.lastIndex
            Else
                sender.record.animList = animList
                For attrIndex As Integer = 0 To handle.Count - 1
                    Dim aCl = tabs(TabControl1.SelectedIndex).attributeClasses(attrIndex)
                    If aCl.type = AttributeType.attrAnim Then
                        handle(attrIndex).items.clear
                        For animIndex = 0 To sender.record.animList.count - 1
                            handle(attrIndex).items.add(sender.record.animList(animIndex))
                        Next
                    ElseIf aCl.type = AttributeType.attrAnimmulti Then
                        For Each cb In handle(attrIndex).controls
                            cb.items.clear
                            For animIndex = 0 To sender.record.animList.count - 1
                                cb.items.add(sender.record.animList(animIndex))
                            Next
                        Next
                    End If
                Next
                sender.lastIndex = sender.selectedIndex
            End If

        End If

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

    Friend Class DataGridViewAnimMulti
        Inherits DataGridView

        Public senderAttr As Integer
        Public recordIndex As Integer
        Public rowIndex As Integer

    End Class

    Friend Class DataGridViewComboBoxEditingControlAnim
        Inherits System.Windows.Forms.DataGridViewComboBoxEditingControl

        Public prevValue As Integer
        Public senderAttr As Integer
        Public recordIndex As Integer
        Public progChange As Boolean

    End Class



    Friend Class DGVButton
        Inherits Button

        Public attrIndex As Integer
        Public record As Record
        Public recordIndex As Integer

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

        Friend Class UnscrollableCarComboBox
            Inherits UnscrollableComboBox

            Public lastIndex As Integer
            Public record As Record
            Public attrIndex As Integer
            Public progChange As Boolean

            Public Sub New()
                progChange = False
            End Sub

        End Class

        Friend Class UnscrollableAnimComboBox
            Inherits UnscrollableComboBox

            Public prevValue As Integer
            Public senderAttr As Integer
            Public recordIndex As Integer
        Public progChange As Boolean
        Public senderCtrl As Integer

        Public Sub New()
                progChange = False
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
                Dim active As Boolean = True

                If tabs(TabControl1.SelectedIndex).attributeClasses(index).validation = False Then active = False
                If tabs(TabControl1.SelectedIndex).attributeClasses(index).hidden = True Then active = False
                If tabs(TabControl1.SelectedIndex).attributeClasses(index).swimmer = True Then
                    If handle(tabs(TabControl1.SelectedIndex).getAttrIndex("canswim")).selectedIndex = 1 Then
                        active = False
                    End If
                End If
                If tabs(TabControl1.SelectedIndex).attributeClasses(index).AIdependant = True Then

                    If Not aiList(handle(tabs(TabControl1.SelectedIndex).getAttrIndex("clone")).selectedIndex).active.Contains(tabs(TabControl1.SelectedIndex).attributeClasses(index).resName) Then
                        active = False
                    End If

                End If

                If active = True Then
                    If Not validateField(sender.handle2(index), tabs(TabControl1.SelectedIndex).attributeClasses(index).type) Then
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
                'If line.Contains("hunterinfo {") Then readData(, line)
                If line.Contains("oldambients {") Then readData(AmbientTab, line)
                If line.Contains("corpseambients {") Then readData(AmbientCorpseTab, line)
                If line.Contains("characters {") Then readData(HuntableTab, line)
                If line.Contains("mapambients {") Then readData(MapAmbientTab, line)
                If line.Contains("prices {") Then readPrices(line)
                line = LineInput(1)
            Loop Until line = "."

            FileClose(1)
        End Sub

        Private Sub readData(ByVal tabIndex As Integer, ByVal line As String)
            line = LineInput(1)
            Do
                If line.Contains("{") Then
                    tabs(tabIndex).addRecord()
                    line = LineInput(1) 'skip past this line, else { will be mistaken for a subsection
                    Do
                        If line.Contains("=") Then

                            Dim resName As String = Replace(Trim(line.Substring(0, line.IndexOf("="))), Chr(0), "")
                            Dim attrIndex = tabs(tabIndex).getAttrIndex(resName)
                            If attrIndex >= 0 Then

                                Dim value
                                Select Case tabs(tabIndex).attributeClasses(attrIndex).type
                                    Case AttributeType.attrString
                                        value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                                    Case AttributeType.attrFile
                                        value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                                    Case AttributeType.attrCar
                                        value = Trim(line.Substring(line.IndexOf("'") + 1)).TrimEnd(CChar("'"))
                                    Case AttributeType.attrInteger
                                    value = Trim(line.Substring(line.IndexOf("=") + 1))
                                Case AttributeType.attrScale
                                    value = Trim(line.Substring(line.IndexOf("=") + 1))
                                Case AttributeType.attrScaleA
                                    value = Trim(line.Substring(line.IndexOf("=") + 1))
                                Case AttributeType.attrFearCall
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrAnim
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrAnimMulti
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrSlot
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrAI
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrTogglableInteger
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrDouble
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrSpd
                                        value = Trim(line.Substring(line.IndexOf("=") + 1))
                                    Case AttributeType.attrBoolean
                                        If line.Contains("TRUE") Then value = True Else value = False
                                    Case AttributeType.attrSwimmer
                                        If line.Contains("TRUE") Then value = True Else value = False
                                    Case AttributeType.attrIntBool
                                        If Trim(line.Substring(line.IndexOf("=") + 1)) > 0 Then value = True Else value = False
                                End Select
                                Dim recordIndex = tabs(tabIndex).records.Count - 1

                                If tabs(tabIndex).attributeClasses(attrIndex).type = AttributeType.attrFile Or tabs(tabIndex).attributeClasses(attrIndex).type = AttributeType.attrCar Then
                                    Dim newFile As Boolean = True
                                    For index = 0 To tabs(tabIndex).attributeClasses(attrIndex).dInd.Count - 1
                                        If tabs(tabIndex).attributeClasses(attrIndex).dInd(index) = value Then
                                            newFile = False
                                        End If
                                    Next
                                    If newFile = True And tabs(tabIndex).attributeClasses(attrIndex).hidden = False Then
                                        tabs(tabIndex).attributeClasses(attrIndex).dInd.Add(value)
                                        tabs(tabIndex).attributeClasses(attrIndex).data.Add(My.Computer.FileSystem.ReadAllBytes(dir & tabs(tabIndex).getAttrClass(resName).gameFolder & value))
                                        If tabs(tabIndex).attributeClasses(attrIndex).type = AttributeType.attrCar Then

                                            tabs(tabIndex).records(recordIndex).animList = readAnimations(tabs(tabIndex).attributeClasses(attrIndex).data(tabs(tabIndex).attributeClasses(attrIndex).data.Count - 1))

                                        End If
                                    End If
                                End If
                                tabs(tabIndex).setAttr(recordIndex, resName, value)

                            End If
                        ElseIf line.Contains("{") Then
                            Dim resName As String = Trim(line.Substring(0, line.IndexOf("{")))
                            Dim attrIndex = tabs(tabIndex).getAttrIndex(resName)
                            '^ make this a contains, not equals for override/addition subsections

                            If attrIndex >= 0 Then

                                'code here!!!!!

                            Else
                                Dim layer As Integer = 1
                                Do
                                    line = LineInput(1)
                                    If line.Contains("{") Then layer += 1
                                    If line.Contains("}") Then layer -= 1
                                Loop Until layer <= 0
                            End If

                        End If
                        line = LineInput(1)
                    Loop Until line.Contains("}")
                    If debug Then
                        printLog("READ " & tabs(tabIndex).nameS & " : " & tabs(tabIndex).records.Count - 1)
                        For atrIndex As Integer = 0 To tabs(tabIndex).attributeClasses.Count - 1
                            If tabs(tabIndex).attributeClasses(atrIndex).type = AttributeType.attrFearCall Or
                        tabs(tabIndex).attributeClasses(atrIndex).type = AttributeType.attrAnimMulti Then
                                'Dim o As Object = tabs(tabIndex).records(tabs(tabIndex).records.Count - 1).attributes(atrIndex).value
                                'printLog(tabs(tabIndex).attributeClasses(atrIndex).displayName & "=" & o(o.count - 1))
                            Else
                                printLog(tabs(tabIndex).attributeClasses(atrIndex).displayName & "=" & tabs(tabIndex).records(tabs(tabIndex).records.Count - 1).attributes(atrIndex).value)
                            End If
                        Next
                        printLog("---------------------------------------------------------------------")
                    End If
                End If
                line = LineInput(1)
            Loop Until line.Contains("}") Or tabs(tabIndex).records.Count = tabs(tabIndex).recordMax
        End Sub

        Function readAnimations(ByVal data As Byte())
            Dim list As List(Of String) = New List(Of String)

            Dim animCount As Long = bytesToLong(data, 32)
            printLog("ANIM" & animCount)
            Dim vertCount As Long = bytesToLong(data, 40)
            printLog("VERT" & vertCount)
            Dim triCount As Long = bytesToLong(data, 44)
            printLog("TRI" & triCount)
            Dim texSize As Long = bytesToLong(data, 48)
            printLog("TEX" & texSize)

            Dim yLoc = 52 + 64 * triCount + 16 * vertCount + texSize

            For a As Integer = 0 To animCount - 1
                Dim name(31) As Byte
                For i As Integer = 0 To 31
                    name(i) = data(yLoc + i)
                Next
                list.Add(System.Text.Encoding.ASCII.GetString(name).Replace(Chr(0), ""))

                Dim frameCount As Long = bytesToLong(data, yLoc + 36)
                yLoc += 40 + 6 * frameCount * vertCount
            Next
            Return list
        End Function

        Function bytesToLong(ByVal bytes() As Byte, ByVal start As Integer) As Long
            Return bytes(start) _
               + bytes(start + 1) * (256&) _
               + bytes(start + 2) * (256 * 256&) _
               + bytes(start + 3) * (256 * 256& * 256&)
        End Function

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
            Public addButton, removeButton, editButton, upButton, downButton As Button
            Public addToolTip, removeToolTip, editToolTip, upToolTip, downToolTip As ToolTip
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
                                ByVal ext As String, ByVal edit As Boolean, ByVal valida As Boolean, ByVal aiD As Boolean,
                                ByVal _swimmer As Boolean, ByVal help As String)
                attributeClasses.Add(New AttributeClass(name, res, type, defaultValue, min, max, hide, gameFolder, ext, edit, valida, aiD, _swimmer, help))
            End Sub

            Public Sub addAttribute(ByRef attrClass)
                attributeClasses.Add(attrClass)
            End Sub

            Public Sub addRecord()
                records.Add(New Record)
                records(records.Count - 1).attributes = New List(Of Attribute)
                records(records.Count - 1).animList = New List(Of String)
                For attrIndex As Integer = 0 To attributeClasses.Count - 1
                    records(records.Count - 1).attributes.Add(New Attribute(attributeClasses(attrIndex).defaultValue))

                    If attributeClasses(attrIndex).type = AttributeType.attrFearCall Or
                    attributeClasses(attrIndex).type = AttributeType.attrAnimMulti Then
                        records(records.Count - 1).attributes(attrIndex).value = New List(Of Integer)
                    End If

                Next
            End Sub

            Public Sub setAttr(ByVal recordIndex As Integer, ByVal resName As String, ByVal _value As Object)
                For attrIndex As Integer = 0 To attributeClasses.Count - 1
                    If attributeClasses(attrIndex).resName = resName Then
                        If attributeClasses(attrIndex).type = AttributeType.attrFearCall Or
                        attributeClasses(attrIndex).type = AttributeType.attrAnimMulti Then
                            records(recordIndex).attributes(attrIndex).value.add(_value)
                        Else
                            records(recordIndex).attributes(attrIndex).value = _value
                        End If

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
            Public attributes As List(Of Attribute)
            Public animList As List(Of String)
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
            Public swimmer As Boolean 'If true, checks can swim combobox

            Public gameFolder, ext As String 'file data only

            Public helpInfo As String

            Public data As List(Of Byte())
            Public dInd As List(Of String)

            Public AIdependant As Boolean 'If true, checks AI to see whether the value should be disabled

            Public Sub New(ByVal _name As String, ByVal _res As String, ByVal _type As AttributeType, ByVal defVal As Object,
                       ByVal min As Integer, ByVal max As Integer, ByVal _hidden As Boolean, ByVal _gameFolder As String,
                       ByVal _ext As String, ByVal _edit As Boolean, ByVal _validation As Boolean, ByVal aiD As Boolean,
                       ByVal _swimmer As Boolean, ByVal help As String)
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
                AIdependant = aiD
                swimmer = _swimmer

                If type = AttributeType.attrFile Or type = AttributeType.attrCar Then
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

            attrSlot ' AI
            attrAI ' Clone
            attrCar ' Creatures only
            attrAnim
            attrSpd  'speed - can be reset when AI changed - 3 decimal places
        attrSwimmer ' attrBoolean with swim change event

        attrScale ' double with 1dp converts 1000 to 100%
        attrScaleA ' ^ + scale0

        attrFearCall 'select multi ai
            attrAnimMulti 'select multi ai

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

        Public Class AI
            Public name As String
            Public id As Integer
            Public active As List(Of String)
            Public minAnim As Integer
            Public defaultSpeed() As Double

            Public Sub New(ByVal nam As String, ByVal i As Integer, ByVal ma As Integer, ByVal spdNo As Integer)
                name = nam
                id = i
                minAnim = ma
                ReDim defaultSpeed(spdNo)
            End Sub
        End Class

    End Class
