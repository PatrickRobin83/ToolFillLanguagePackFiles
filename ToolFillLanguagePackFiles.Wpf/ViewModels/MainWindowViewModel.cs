/*
*----------------------------------------------------------------------------------
*          Filename:	MainWindowViewModel.cs
*          Date:        2022.10.17
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using ToolFillLanguagePackFiles.Wpf.Views;

namespace ToolFillLanguagePackFiles.Wpf.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

[ObservableObject]
public partial class MainWindowViewModel
{
    #region Fields
    [ObservableProperty]
    private ContenView _cV = new ContenView()
    {
        DataContext = new ContentViewModel()
    };

    #endregion

    #region Properties

    #endregion

    #region Constructor

    #endregion

    #region Methods

    #endregion

    #region Commands

    #endregion
}