   M S B A M L   `   `   `  ����   WPF Archiver$    BestandsArchivering.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Bestands Archiver��$	��500��$	��800�� � WindowStartupLocation$ CenterScreen=�.��� $��15q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��Bronmap:��$��0��$��0��$��Center=�$��0,0,5,5q��� -    ��Name$ txtSourcePath��$��0��$��1�� �� 
IsReadOnly$
 True��$��0,0,5,5q��� -    ��Name$ btnSelectSource��$	��...��$��0��$��2��$��30��$��0,0,0,5q��� $��Doelmap:��$��1��$��0��$��Center=�$��0,0,5,5q��� -   $ txtTargetPath��$��1��$��1��$
 True��$��0,0,5,5q��� -   $ btnSelectTarget��$	��...��$��1��$��2��$��30��$��0,0,0,5q��� $$��Verplaats bestanden ouder dan:��$��2��$��0��$��Center=�$��0,0,5,10q�(  "System.Windows.Controls.DatePicker  -     Name$ dpCutoffDate��$��2��$��1��$
��Left=�$	��150��$��0,0,0,10q��� -   $ btnMoveFiles��$��Verplaats bestanden��$��3��$��0��$��3��$
��10,5q�$��0,0,0,15q�$
��Bold"��� $��4��$��0��$��3�����	#FFDDDDDD �� Padding$	 5,3q�	 �� CornerRadius$	 3,3,0,0��._��� $��Activiteitenlog��$
��Bold"��� -   
 ��Name$
 lstLogs��$��5��$��0��$��3��$��0,0,0,5q�$��0,0,0,1q����� .R��� $
��Wrap=�$��2q����� �� $��6��$��0��$��3�����	#FFEEEEEE.@��� .��� -    �Name$ progressBar��$	��200��$��16�� � Minimum$ 0Q� � Maximum$	 100Q�$y�	Collapsed=��� $��Right=�.���� -	    ��Name$ 	txtStatus��$��Klaar��$	��5,0q�