using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Petzold.Media3D;
using Petzold.Text3D;
using Quixo3D.Framework;

namespace Quixo3D.UI
{
    /// <summary>
    /// A 3D class used to render the axes of an X,Y,Z coordinate system.
    /// </summary>
    public class BoardAxes : ModelVisual3D
    {

        Collection<SolidText> textModels;
        Collection<Cylinder> axesModels;
        bool visible;

        /// <summary>
        /// Creates a new instance of the <see cref="BoardAxes"/> class.
        /// </summary>
        public BoardAxes()
        {
            textModels = new Collection<SolidText>();
            axesModels = new Collection<Cylinder>();

            Cylinder xAxis = CreateXAxis();
            Cylinder zAxis = CreateZAxis();
            Cylinder yAxis = CreateYAxis();
            ModelVisual3D xLabels = CreateXAxisLabels();
            ModelVisual3D zLabels = CreateZAxisLabels();
            ModelVisual3D yLabels = CreateYAxisLabels();

            Children.Add(xAxis);
            Children.Add(zAxis);
            Children.Add(yAxis);
            Children.Add(xLabels);
            Children.Add(zLabels);
            Children.Add(yLabels);

        }

        /// <summary>
        /// Gets or sets whether the axes are visible or hidden.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                bool oldValue = visible;
                visible = value;
                if(oldValue != visible)
                {
                    OnVisibleChanged();
                }
            }
        }

        void OnVisibleChanged()
        {
            if(visible)
            {
                foreach(SolidText textModel in textModels)
                {
                    textModel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.White));
                    textModel.SideMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.White));
                }
                foreach(Cylinder axisModel in axesModels)
                {
                    axisModel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.White));
                }
            }
            else
            {
                foreach(SolidText textModel in textModels)
                {
                    textModel.Material = null;
                    textModel.SideMaterial = null;
                }
                foreach(Cylinder axisModel in axesModels)
                {
                    axisModel.Material = null;
                }
            }
        }
        

        ModelVisual3D CreateXAxisLabels()
        {
            double spacing = ConfigSettings.Instance.CellSpacing;
            double xAlign = -.5;
            double yAlign = .5;
            double zAlign = -4;

            ModelVisual3D model = new ModelVisual3D();

            for(int i = 0;i < Constants.Dimension;i++)
            {
                SolidText text = new SolidText();
                text.Text = i.ToString();
                text.FontFamily = new FontFamily("Consolas");
                text.FontSize = 1;
                text.Depth = .1;

                double amount = spacing * i;
                text.Transform = new TranslateTransform3D(amount + xAlign, yAlign, zAlign);
                model.Children.Add(text);
                textModels.Add(text);
            }

            SolidText axisLabel = new SolidText();
            axisLabel.Text = "X";
            axisLabel.FontFamily = new FontFamily("Consolas");
            axisLabel.FontSize = 1;
            axisLabel.Depth = .1;
            Transform3DGroup transforms = new Transform3DGroup();
            transforms.Children.Add(new TranslateTransform3D(spacing * Constants.Dimension + xAlign*4, yAlign / 2, zAlign));
            AxisAngleRotation3D rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            transforms.Children.Add(new RotateTransform3D(rotation));
            axisLabel.Transform = transforms;
            model.Children.Add(axisLabel);
            textModels.Add(axisLabel);            

            return model;
        }

        ModelVisual3D CreateZAxisLabels()
        {
            double spacing = ConfigSettings.Instance.CellSpacing;
            double xAlign = -4;
            double yAlign = .5;
            double zAlign = 0;

            ModelVisual3D model = new ModelVisual3D();

            for(int i = 0;i < Constants.Dimension;i++)
            {
                SolidText text = new SolidText();
                text.Text = i.ToString();
                text.FontFamily = new FontFamily("Consolas");
                text.FontSize = 1;
                text.Depth = .1;

                double amount = spacing * i;
                text.Transform = new TranslateTransform3D(xAlign, yAlign, amount + zAlign);
                model.Children.Add(text);
                textModels.Add(text);
            }
            SolidText axisLabel = new SolidText();
            axisLabel.Text = "Z";
            axisLabel.FontFamily = new FontFamily("Consolas");
            axisLabel.FontSize = 1;
            axisLabel.Depth = .1;
            axisLabel.Transform = new TranslateTransform3D(xAlign, yAlign / 2, spacing * Constants.Dimension - 2);
            model.Children.Add(axisLabel);
            textModels.Add(axisLabel);            

            return model;
        }

        ModelVisual3D CreateYAxisLabels()
        {
            double spacing = ConfigSettings.Instance.CellSpacing;
            double xAlign = -4;
            double yAlign = .5;
            double zAlign = -4;

            ModelVisual3D model = new ModelVisual3D();

            for(int i = 0;i < Constants.Dimension;i++)
            {
                SolidText text = new SolidText();
                text.Text = i.ToString();
                text.FontFamily = new FontFamily("Consolas");
                text.FontSize = 1;
                text.Depth = .1;

                double amount = spacing * i;
                text.Transform = new TranslateTransform3D(xAlign, yAlign + amount, zAlign);
                model.Children.Add(text);
                textModels.Add(text);
            }
            SolidText axisLabel = new SolidText();
            axisLabel.Text = "Y";
            axisLabel.FontFamily = new FontFamily("Consolas");
            axisLabel.FontSize = 1;
            axisLabel.Depth = .1;
            axisLabel.Transform = new TranslateTransform3D(xAlign, spacing * Constants.Dimension - yAlign*2, zAlign);
            model.Children.Add(axisLabel);
            textModels.Add(axisLabel);            


            return model;
        }

        Cylinder CreateBaseAxis()
        {
            Cylinder axis = new Cylinder();

            Transform3DGroup transforms = new Transform3DGroup();
            ScaleTransform3D scaleTransform = new ScaleTransform3D(.05, 25, .05);
            transforms.Children.Add(scaleTransform);

            axis.Transform = transforms;
            axesModels.Add(axis);
            return axis;
        }

        Cylinder CreateXAxis()
        {
            Cylinder xAxis = CreateBaseAxis();
            Transform3DGroup xAxisTransforms = xAxis.Transform as Transform3DGroup;
            TranslateTransform3D moveTransform = new TranslateTransform3D(-3, -3, -3);
            RotateTransform3D rotateTransform = new RotateTransform3D();
            AxisAngleRotation3D rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90);

            rotateTransform.Rotation = rotation;
            xAxisTransforms.Children.Add(rotateTransform);
            xAxisTransforms.Children.Add(moveTransform);
            xAxis.Transform = xAxisTransforms;
            
            return xAxis;

        }

        Cylinder CreateZAxis()
        {
            Cylinder zAxis = CreateBaseAxis();
            Transform3DGroup zAxisTransforms = zAxis.Transform as Transform3DGroup;

            RotateTransform3D rotateTransform = new RotateTransform3D();
            AxisAngleRotation3D rotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
            rotateTransform.Rotation = rotation;

            TranslateTransform3D moveTransform = new TranslateTransform3D(-3, -3, -3);
            zAxisTransforms.Children.Add(rotateTransform);
            zAxisTransforms.Children.Add(moveTransform);
            zAxis.Transform = zAxisTransforms;
            return zAxis;

        }

        Cylinder CreateYAxis()
        {
            Cylinder yAxis = CreateBaseAxis();
            Transform3DGroup yAxisTransforms = yAxis.Transform as Transform3DGroup;
            ScaleTransform3D scale = new ScaleTransform3D(1, .9, 1);
            TranslateTransform3D moveTransform = new TranslateTransform3D(-3, -3, -3);

            yAxisTransforms.Children.Add(scale);
            yAxisTransforms.Children.Add(moveTransform);
            yAxis.Transform = yAxisTransforms;
            return yAxis;

        }

    }
}
