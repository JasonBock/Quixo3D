using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Petzold.Media3D;
using Quixo3D.Framework;
using System.Windows.Media.Animation;

namespace Quixo3D.UI {

    public class GamePieceVisual : ModelVisual3D {

        private GeometryModel3D emptyModel;
        private GeometryModel3D xModel;
        private GeometryModel3D oModel;
        private PieceState pieceState;
        private PieceType pieceType;
        private bool highlighted;

        /// <summary>
        /// Creates a new instance of a <see cref="GamePieceVisual"/>.
        /// </summary>
        public GamePieceVisual() {

            this.pieceType = PieceType.Empty;

            emptyModel = CreateEmptyModel();
            xModel = CreateXModel();
            oModel = CreateOModel();

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(emptyModel);
            group.Children.Add(xModel);
            group.Children.Add(oModel);

            this.Content = group;

            Redraw();
        }

        public bool Highlighted
        {
            get { return highlighted; }
            set { highlighted = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="PieceType"/>.
        /// </summary>
        public PieceType PieceType {
            get { return pieceType; }
            set { pieceType = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="PieceState"/>.
        /// </summary>
        public PieceState PieceState {
            get { return pieceState; }
            set { pieceState = value; }
        }

        /// <summary>
        /// Causes the piece to change its appearance
        /// given its current <see cref="PieceType"/> and
        /// <see cref="PieceState"/> values.
        /// </summary>
        public void Redraw() {

            if(pieceType == PieceType.Empty) {
                HideModel(xModel);
                HideModel(oModel);
                ShowModel(emptyModel);
            }

            if(pieceType == PieceType.X) {
                HideModel(emptyModel);
                HideModel(oModel);
                ShowModel(xModel);
            }

            if(pieceType == PieceType.O) {
                HideModel(xModel);
                HideModel(emptyModel);
                ShowModel(oModel);
            }
        

        }

        public GamePieceVisualInfo GetInfoObject(GameBoardController controller)
        {
            GamePieceVisualInfo info = new GamePieceVisualInfo();

            info.PieceState = this.PieceState;
            info.PieceType = this.PieceType;
            info.XMaterial = this.xModel.Material;
            info.EmptyMaterial = this.emptyModel.Material;
            info.Coordinate = controller.GetCoordinateFromGamePiece(this);
            info.GamePiece = this;
            return info;
        }

        void ShowModel(GeometryModel3D model) {

            SolidColorBrush frontBrush = new SolidColorBrush();
            SolidColorBrush backBrush = new SolidColorBrush();
            backBrush.Color = Colors.White;

            double targetOpacity = ConfigSettings.Instance.NonEmptyCellOpacity;
            
            switch(pieceState) {
                case PieceState.Nothing:
                    frontBrush.Color = Colors.White;
                    targetOpacity = ConfigSettings.Instance.EmptyCellOpacity;
                    break;
                case PieceState.ValidSource:
                    if(highlighted)
                    {
                        frontBrush.Color = Colors.Yellow;
                    }
                    else
                    {
                        if(PieceType != PieceType.Empty)
                        {
                            frontBrush.Color = Colors.Chartreuse;
                        }
                        else
                        {
                            frontBrush.Color = Colors.LightGreen;
                        }
                    }
                    break;
                case PieceState.ValidDestination:
                    frontBrush.Color = Colors.Blue;
                    break;
                case PieceState.Placed:
                    frontBrush.Color = Colors.Red;
                    break;
            }

            frontBrush.Opacity = targetOpacity;
            backBrush.Opacity = 1;
            ApplyColor(model, frontBrush, backBrush);
       }

        void ApplyColor(GeometryModel3D model, Brush frontBrush, Brush backBrush) {
            MaterialGroup group = new MaterialGroup();
            DiffuseMaterial material = new DiffuseMaterial(frontBrush);
            SpecularMaterial specular = new SpecularMaterial(backBrush, 1000);
            group.Children.Add(material);
            group.Children.Add(specular);

            model.Material = group;
            model.BackMaterial = group;
        }

        void HideModel(GeometryModel3D model) {
            model.Material = null;
            model.BackMaterial = null;
        }

        void AssignDefaultMaterial(GeometryModel3D model) {
            model.Material = new MaterialGroup();
        }

        GeometryModel3D CreateEmptyModel() {
            CubeMesh cubeMesh = new CubeMesh();
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = cubeMesh.Geometry;
            return model;
        }

        GeometryModel3D CreateXModel() {
            CubeMesh cubeMesh = new CubeMesh();
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = cubeMesh.Geometry;
            model.Transform = new ScaleTransform3D(.2, 1, 1);
            return model;
        }

        GeometryModel3D CreateOModel() {
            TorusMesh mesh = new TorusMesh();
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh.Geometry;
            return model;
        }
    }

}
