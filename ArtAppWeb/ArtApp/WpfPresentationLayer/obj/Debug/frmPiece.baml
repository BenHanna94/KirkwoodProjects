   M S B A M L   `   `   `  ����O  KWpfPresentationLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:WpfPresentationLayerWpfPresentationLayer  5   	   #    WpfPresentationLayer.frmPiece   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:WpfPresentationLayer     5   	   -   5          � Title$  Piece Details��5   	   $	��600��6   $	��650��6+   
������� � 	Resources 5
       �� 
TargetType �� Property �� Value��%T   '��       �� 5   
   " ��6   .�5      ��  � ��
 �����6@   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��25 �6   6*   �� 5      $��40 �6   6*   �� 5      $��40 �6   6*   �� 5      $��40 �6   6*   �� 5      $��80 �6   6*   �� 5      $��40 �6   6*   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   6*   �� 5      $��25 �6   6*   �� 5      $	��140 �6   6+   �� 5      $��40 �6   6*   �� 5      $��25 �6   6*    � ColumnDefinitions 5!   
   �� 5"      6!   �� 5#      6!   .+�5%   
   �� $��Center=�6   $��Right=�6+   $��1��6G   $��0 0 10 0q�6T   .��6f   	Piece ID:6q   �� 5&   
   $��Center=�6   $��Right=�6+   $��2��6G   $��0 0 10 0q�6T   .��6f   Project ID:6s   �� 5'   
   $��Center=�6   $��Right=�6+   $��3��6G   $��0 0 10 0q�6T   .��6f   Assigned User:6v   �� 5(   
   $��Center=�6   $��Right=�6+   $��4��6G   $��0 0 10 0q�6T   .��6f   Description:6t   �� 5)   
   $��Center=�6   $��Right=�6+   $��5��6G   $��0 0 10 0q�6T   .��6f   Stage:6n   �� 5*   
   $��Center=�6   $��Right=�6+   $��6��6G   $��0 0 10 0q�6T   .��6f   Compensation:6u   �� 5+   
   $��Center=�6   $��Right=�6+   $��7��6G   $��0 0 10 0q�6T   .��6f   Compensated Status:6{   �� 5,   
   $��Center=�6   $��Right=�6+   $��8��6G   $��0 0 10 0q�6T   .��6f   	Complete:6q   �� 5.   
   -   5         ��Name$ 
txtPieceID��5.      $��Center=�6&   $��5 5 50 5q�6A   $
��10 0q�6S   $��1��6b   $��1��6o   {�.  
�������6�   �� 5/   
   -   5        $ txtProjectID��5/      $��Center=�6(   $��5 5 50 5q�6C   $
��10 0q�6U   $��2��6d   $��1��6q   {�.  
�������6�   �� 50   
   -   5        $ txtAssignedUser��50      $��Center=�6+   $��5 5 50 5q�6F   $
��10 0q�6X   $��3��6g   $��1��6t   {�.  6�   �� 51   
   -   5        $ txtDescription��51      $��60��6*   {�.  $��Center=�6H   $��5 5 50 5q�6c   $
��10 0q�6u   $��4��6�   $��1��6�    �� TextWrapping$ WrapWithOverflow=�6�   6�   �� 52   
   -   5        $ txtStage��52      $��Center=�6$   $��5 5 50 5q�6?   $
��10 0q�6Q   $��5��6`   $��1��6m   {�.  6�   �� 53   
   -   5        $ txtCompensation��53      $��Center=�6+   $��5 5 50 5q�6F   $
��10 0q�6X   $��6��6g   $��1��6t   {�.  6�   �� 55   
   -   5        	 ��Name$	 cmbCompensatedStatus��55      $��Center=�61   $��5 5 50 5q�6L   $
��10 0q�6^   $��7��6m   $��1��6z   
 �� Text$

 Test��6�   {�.   �� 
IsEditable .  �� 
IsReadOnly . 6�   �� 57   
   -	   5         ��Name$ chkComplete��57      $��8��6(   $��1��65   $��Center=�6E   $	��5 5q�6`   {�.  6�   �� 59   
   $��9��6   $��Right=�6   $
��20 0q�69   $��10(�6G   $��Bottom=�6U   .��6p   Unassigned References6�   �� 5:   
   $��9��6   $��1��6   $
��Left=�6-   $
��20 0q�6H   $��10(�6V   $��Bottom=�6d   .��6   Assigned References6�   �� 5<   
   -
   5         ��Name$ lstUnassignedRefs��5<      $	��150��6-   $��10��69   $��0��6G   $��Right=�6W   $��5q�6s   $��5q�6~   {�.  6�   �� 5=   
   -   5        $ lstAssignedRefs��5=      $	��150��6+   $��10��67   $��1��6E   $
��Left=�6U   $��5q�6q   $��5q�6|   {�.  6�   �� 5?   
   -   5         ��Name$ btnEdit��5?      $��25��6"   $��50��6.   $
��10 0q�69   $��Right=�6G   $��Center=�6c   $��11��6~   $
��Edit��6�   6�   �� 5@   
   -   5        $ btnSave��5@      $��25��6"   $��50��6.   $
��10 0q�69   $��Right=�6G   $��Center=�6c   $��11��6~   $
��Save��6�   $y�Hidden=�6�   6�   �� 5A   
   -   5        $ 	btnCancel��5A      $��25��6$   $��50��60   $
��10 0q�6;   $
��Left=�6I   $��Center=�6d   $��1��6   $��11��6�   $��Cancel��6�   6�   5C      5D      