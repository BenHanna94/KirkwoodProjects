   M S B A M L   `   `   `  ����O  KWpfPresentationLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:WpfPresentationLayerWpfPresentationLayer  5   	   ,    &WpfPresentationLayer.frmUpdatePassword   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:WpfPresentationLayer     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ Update Password��5   	   $	��300��6!   $	��450��6.   
�������.��5	      �  � RowDefinitions 5
   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      $	��250 �6   6-   �� 5      6!   .+�5   
   �� -   5         ��Name$ lblCurrentPassword��5      $��0��5      $��1��6    $��Center=�5      $��Right=�5      $��Current Password��5      $��Center=�5      $��Right=�5      $
��10 0q�5      $��DemiBold"�5      �� 5   
   -   5        $ lblNewPassword��5      $��0��5       $��2��6    $��Center=�5!      $��Right=�5"      $��New Password��5#      $��Center=�5$      $��Right=�5%      $
��10 0q�5&      $��DemiBold"�5'      �� 5(   
   -   5        $ lblRetypetPassword��5(      $��0��5)      $��3��6    $��Center=�5*      $��Right=�5+      $��Retype New Password��5,      $��Center=�5-      $��Right=�5.      $
��10 0q�5/      $��DemiBold"�50      �� 51   
   -   5         ��Name$ 	btnSubmit��51      $��0��52      $��4��6!   $��Submit��53      $
��10 0q�54      $	��5 9q�55      $��Right=�56      $��Center=�57      $��DemiBold"�58       �� 	IsDefault . 6?   G� 59   
   -   5         G�Name$ pwdCurrentPassword��59      $��1��5:      $��1��6#   $	��150��5;      $��26��6"   $
��10 5q�5<      $��Center=�5=      $��Center=�5>      $
��Left=�5?      G� 5A   
   -   5        $ pwdNewPassword��5A      $��2��5B      $��2��6#   $	��150��5C      $��26��6"   $
��10 5q�5D      $��Center=�5E      $��Center=�5F      $
��Left=�5G      G� 5I   
   -   5        $ pwdRetypePassword��5I      $��3��5J      $��3��6#   $	��150��5K      $��26��6"   $
��10 5q�5L      $��Center=�5M      $��Center=�5N      $
��Left=�5O      5Q      5R      