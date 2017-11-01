<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("script 1")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("AU", New System.Windows.Forms.TreeNode() {TreeNode1})
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Script 2")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Sabre", New System.Windows.Forms.TreeNode() {TreeNode2, TreeNode3})
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Amadeus")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Galileo")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Root", New System.Windows.Forms.TreeNode() {TreeNode4, TreeNode5, TreeNode6})
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Sabre")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Amadeus")
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Galileo")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Root", New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode9, TreeNode10})
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TreeView2 = New System.Windows.Forms.TreeView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout
        '
        'TreeView1
        '
        Me.TreeView1.Location = New System.Drawing.Point(12, 32)
        Me.TreeView1.Name = "TreeView1"
        TreeNode1.Name = "Node0"
        TreeNode1.Text = "script 1"
        TreeNode2.Name = "Node1"
        TreeNode2.Text = "AU"
        TreeNode3.Name = "Node2"
        TreeNode3.Text = "Script 2"
        TreeNode4.Name = "Node1"
        TreeNode4.Text = "Sabre"
        TreeNode5.Name = "Node2"
        TreeNode5.Text = "Amadeus"
        TreeNode6.Name = "Node3"
        TreeNode6.Text = "Galileo"
        TreeNode7.Name = "Node0"
        TreeNode7.Text = "Root"
        Me.TreeView1.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode7})
        Me.TreeView1.Size = New System.Drawing.Size(326, 420)
        Me.TreeView1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(667, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Available Commands"
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(385, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Script Commands"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(386, 32)
        Me.TextBox2.Multiline = true
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(214, 420)
        Me.TextBox2.TabIndex = 3
        '
        'TreeView2
        '
        Me.TreeView2.Location = New System.Drawing.Point(673, 37)
        Me.TreeView2.Name = "TreeView2"
        TreeNode8.Name = "Node1"
        TreeNode8.Text = "Sabre"
        TreeNode9.Name = "Node2"
        TreeNode9.Text = "Amadeus"
        TreeNode10.Name = "Node3"
        TreeNode10.Text = "Galileo"
        TreeNode11.Name = "Node0"
        TreeNode11.Text = "Root"
        Me.TreeView2.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode11})
        Me.TreeView2.Size = New System.Drawing.Size(207, 420)
        Me.TreeView2.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(12, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Scripts"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(891, 517)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TreeView2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TreeView1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TreeView2 As TreeView
    Friend WithEvents Label3 As Label
End Class
