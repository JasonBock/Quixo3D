   M S B A M L   `   `   `  ���� L  HQuixo3D.Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:Quixo3D.Framework;assembly=Quixo3D.FrameworkQuixo3D.Framework   
Quixo3D.UI   Quixo3D.UI.MoveHistory   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Jquixo:clr-namespace:Quixo3D.Framework;assembly=Quixo3D.Framework      � 	Resources     itemDataTemplate � ColumnDefinitions z� SharedSizeGroup �� Path 
 Header �� 
TargetType �� Padding  multiColumnListBox �� 
TargetType �� Property �� Value	 � IsSharedSizeScope
 � RowDefinitions K� IsItemsHost��%_  &	        &	 .    &	 a    7  7  7  �� .R�� $��12(�$��3q� �� $
��Auto �$ PlayerColumn���� $
��Auto �$ SourceColumn���� $
��Auto �$ 
DestColumn��.+��� $��0��$��	10,0,10,0q����� $ 
PlayerText��� $��1��$��	10,0,10,0q����� $ Source��� $��2��$��	10,0,10,0q����� $ Destination��� " ��.R��� $��12(�$ 	10,3,10,4q�._��� �� " ��.���  � �� �� .R�� 	 . $��Stretch=�
 �� $
��Auto ��� $��* �.+��� $��1��._��� $��10q�._�]� .+��� $	��Top=�$��Hidden=�.��� $	��Top=�$��0,0,18,0q� �� $
��Auto �$ PlayerColumn���� $
��Auto �$ SourceColumns���� $
��Auto �$ 
DestColumn��.+��� $��0��8��  .��Player�� $��1��8�� .��Source�� $��2��8�� .��Destination�� $��Master��$��Hidden=�$��Visible=�.����  . .���� -    ��Name$ itemList��$��Stretch=� �� SelectedIndex$ 0��#��[ #��[  