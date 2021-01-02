<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Tab_Areas = New System.Windows.Forms.TabPage()
        Me.Tab_Huntables = New System.Windows.Forms.TabPage()
        Me.Tab_Ambients = New System.Windows.Forms.TabPage()
        Me.Tab_AmbientCorpses = New System.Windows.Forms.TabPage()
        Me.Tab_MapAmbients = New System.Windows.Forms.TabPage()
        Me.Tab_Weapons = New System.Windows.Forms.TabPage()
        Me.Tab_Equipment = New System.Windows.Forms.TabPage()
        Me.Tab_Other = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Tab_Areas)
        Me.TabControl1.Controls.Add(Me.Tab_Huntables)
        Me.TabControl1.Controls.Add(Me.Tab_Ambients)
        Me.TabControl1.Controls.Add(Me.Tab_AmbientCorpses)
        Me.TabControl1.Controls.Add(Me.Tab_MapAmbients)
        Me.TabControl1.Controls.Add(Me.Tab_Weapons)
        Me.TabControl1.Controls.Add(Me.Tab_Equipment)
        Me.TabControl1.Controls.Add(Me.Tab_Other)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(915, 809)
        Me.TabControl1.TabIndex = 0
        '
        'Tab_Areas
        '
        Me.Tab_Areas.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Areas.Name = "Tab_Areas"
        Me.Tab_Areas.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Areas.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Areas.TabIndex = 4
        Me.Tab_Areas.Text = "Areas"
        Me.Tab_Areas.UseVisualStyleBackColor = True
        '
        'Tab_Huntables
        '
        Me.Tab_Huntables.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Huntables.Name = "Tab_Huntables"
        Me.Tab_Huntables.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Huntables.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Huntables.TabIndex = 0
        Me.Tab_Huntables.Text = "Huntable Creatures"
        Me.Tab_Huntables.UseVisualStyleBackColor = True
        '
        'Tab_Ambients
        '
        Me.Tab_Ambients.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Ambients.Name = "Tab_Ambients"
        Me.Tab_Ambients.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Ambients.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Ambients.TabIndex = 1
        Me.Tab_Ambients.Text = "Ambient Creatures"
        Me.Tab_Ambients.UseVisualStyleBackColor = True
        '
        'Tab_AmbientCorpses
        '
        Me.Tab_AmbientCorpses.Location = New System.Drawing.Point(8, 82)
        Me.Tab_AmbientCorpses.Name = "Tab_AmbientCorpses"
        Me.Tab_AmbientCorpses.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_AmbientCorpses.Size = New System.Drawing.Size(899, 719)
        Me.Tab_AmbientCorpses.TabIndex = 7
        Me.Tab_AmbientCorpses.Text = "Ambient Corpses"
        Me.Tab_AmbientCorpses.UseVisualStyleBackColor = True
        '
        'Tab_MapAmbients
        '
        Me.Tab_MapAmbients.Location = New System.Drawing.Point(8, 82)
        Me.Tab_MapAmbients.Name = "Tab_MapAmbients"
        Me.Tab_MapAmbients.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_MapAmbients.Size = New System.Drawing.Size(899, 719)
        Me.Tab_MapAmbients.TabIndex = 2
        Me.Tab_MapAmbients.Text = "Map Ambient Creatures"
        Me.Tab_MapAmbients.UseVisualStyleBackColor = True
        '
        'Tab_Weapons
        '
        Me.Tab_Weapons.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Weapons.Name = "Tab_Weapons"
        Me.Tab_Weapons.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Weapons.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Weapons.TabIndex = 3
        Me.Tab_Weapons.Text = "Weapons"
        Me.Tab_Weapons.UseVisualStyleBackColor = True
        '
        'Tab_Equipment
        '
        Me.Tab_Equipment.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Equipment.Name = "Tab_Equipment"
        Me.Tab_Equipment.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Equipment.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Equipment.TabIndex = 5
        Me.Tab_Equipment.Text = "Equipment"
        Me.Tab_Equipment.UseVisualStyleBackColor = True
        '
        'Tab_Other
        '
        Me.Tab_Other.Location = New System.Drawing.Point(8, 82)
        Me.Tab_Other.Name = "Tab_Other"
        Me.Tab_Other.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Other.Size = New System.Drawing.Size(899, 719)
        Me.Tab_Other.TabIndex = 6
        Me.Tab_Other.Text = "Other"
        Me.Tab_Other.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(914, 807)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.Text = "Carnivores Mod Manager v0.1.0"
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents Tab_Huntables As TabPage
    Friend WithEvents Tab_Ambients As TabPage
    Friend WithEvents Tab_MapAmbients As TabPage
    Friend WithEvents Tab_Weapons As TabPage
    Friend WithEvents Tab_Areas As TabPage
    Friend WithEvents Tab_Equipment As TabPage
    Friend WithEvents Tab_Other As TabPage
    Friend WithEvents Tab_AmbientCorpses As TabPage
End Class
