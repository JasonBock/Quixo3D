   M S B A M L   `   `   `  ����L  HQuixo3D.Framework, Version=0.1.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:Quixo3D.Framework;assembly=Quixo3D.FrameworkQuixo3D.Framework  5       
Quixo3D.UI   Quixo3D.UI.MoveHistory   5      W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      6x,http://schemas.microsoft.com/winfx/2006/xaml   5      Hquixo:clr-namespace:Quixo3D.Framework;assembly=Quixo3D.Framework   5        � 	Resources  5         itemDataTemplate � ColumnDefinitions z� SharedSizeGroup �� Path 
 Header �� 
TargetType �� Padding  multiColumnListBox �� 
TargetType �� Property �� Value	 � IsSharedSizeScope
 � RowDefinitions K� IsItemsHost��%  &	        &	 �    &	 U    7  7  7  �� 5      .R�5	      � $��12(�6
   $��3q�6"    5
      �� 5      $
��Auto �6   $ PlayerColumn��6%   �� 5      $
��Auto �6   $ SourceColumn��6%   �� 5      $
��Auto �6   $ 
DestColumn��6%   .+�5      �� $��0��6   $��	10,0,10,0q�6    ��63   �� $ 
PlayerText��� 5      $��1��6   $��	10,0,10,0q�6    ��63   �� $ Source��� 5      $��2��6   $��	10,0,10,0q�6    ��63   �� $ Destination�5      5      �� 5      " ��6#   .R�5      �� $��12(�6   $ 	10,3,10,4q�6$   ._�5      �� 5      5      �� 5      " ��6%   .�5      ��  � �� 5      �� 5       .R�5!      � 	 . $��Stretch=�6+   
 5"   	   �� 5#   
   $
��Auto �6   �� 5$   
   $��* �6   .+�5&   	   �� $��1��6   ._�5'   
   �� $��10q�6   ._�5(      ]� .+�5)      �� $	��Top=�6   $��Hidden=�6.   .��5*      � $	��Top=�6   $��0,0,18,0q�6'    5+      �� 5,      $
��Auto �6    $ PlayerColumn��6-   �� 5-      $
��Auto �6    $ SourceColumns��6-   �� 5.      $
��Auto �6    $ 
DestColumn��6-   .+�50      �� $��0��6   8��  .��6H   Player6P   �� 51      $��1��6   8�� .��6I   Source6Q   �� 52      $��2��6   8�� .��6H   Destination6U   53      54      �� 55      $��Master��6h   $��Hidden=�6   $��Visible=�6A   .��56      ��  . 6   57      58      59      5:   
   5;   	   5<      5>      5?      5A      .��5C      �� -   5         ��Name$ itemList��5C      $��Stretch=�6    �� SelectedIndex$ 0��69   #��[ 6K   #��[  6w   5E      