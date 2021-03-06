﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.Nodes;
using Materia.Nodes.Atomic;
using Materia.Imaging;
using System.Drawing;
using System.IO;

namespace Materia.Exporters
{
    public class Unreal4Exporter : Exporter
    {
        Graph graph;
        public Unreal4Exporter(Graph g)
        {
            graph = g;
        }

        public override void ExportSync(string path)
        {
            RawBitmap mroh = null;

            string name = graph.Name;

            foreach (var s in graph.OutputNodes)
            {
                Node n = null;

                if (graph.NodeLookup.TryGetValue(s, out n))
                {
                    if (n is OutputNode)
                    {
                        OutputNode on = n as OutputNode;

                        if (on.OutType == OutputType.basecolor)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                bmp = new RawBitmap(on.Width, on.Height, bits);
                                var src = bmp.ToBitmap();

                                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_color.png"), FileMode.OpenOrCreate))
                                {
                                    src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                src.Dispose();
                            }
                        }
                        else if (on.OutType == OutputType.normal)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                bmp = new RawBitmap(on.Width, on.Height, bits);
                                var src = bmp.ToBitmap();

                                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_normal.png"), FileMode.OpenOrCreate))
                                {
                                    src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                src.Dispose();
                            }
                        }
                        else if (on.OutType == OutputType.metallic)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                mroh.CopyRedToBlue(tmp);
                            }
                        }
                        else if (on.OutType == OutputType.roughness)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                mroh.CopyRedToGreen(tmp);
                            }
                        }
                        else if (on.OutType == OutputType.occlusion)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                mroh.CopyRedToRed(tmp);
                            }
                        }
                        else if (on.OutType == OutputType.height)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                mroh.CopyRedToAlpha(tmp);
                            }
                        }
                        else if (on.OutType == OutputType.thickness)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                bmp = new RawBitmap(on.Width, on.Height, bits);
                                var src = bmp.ToBitmap();

                                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_thickness.png"), FileMode.OpenOrCreate))
                                {
                                    src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                src.Dispose();
                            }
                        }
                        else if(on.OutType == OutputType.emission)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                bmp = new RawBitmap(on.Width, on.Height, bits);
                                var src = bmp.ToBitmap();

                                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_emission.png"), FileMode.OpenOrCreate))
                                {
                                    src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                src.Dispose();
                            }
                        }
                    }
                }
            }

            if (mroh != null)
            {
                var src = mroh.ToBitmap();

                using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_occroughmetheight.png"), FileMode.OpenOrCreate))
                {
                    src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }

                src.Dispose();
            }
        }

        public override Task Export(string path)
        {
            RawBitmap mroh = null;

            int i = 0;

            string name = graph.Name;

            Queue<Task> runningTasks = new Queue<Task>();

            foreach (var s in graph.OutputNodes)
            {
                Node n = null;

                if (graph.NodeLookup.TryGetValue(s, out n))
                {
                    if (n is OutputNode)
                    {
                        OutputNode on = n as OutputNode;

                        if (on.OutType == OutputType.basecolor)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    bmp = new RawBitmap(on.Width, on.Height, bits);
                                    var src = bmp.ToBitmap();

                                    using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_color.png"), FileMode.OpenOrCreate))
                                    {
                                        src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                    }

                                    src.Dispose();
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.normal)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {

                                var t = Task.Run(() =>
                                {
                                    bmp = new RawBitmap(on.Width, on.Height, bits);
                                    var src = bmp.ToBitmap();

                                    using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_normal.png"), FileMode.OpenOrCreate))
                                    {
                                        src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                    }

                                    src.Dispose();
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.metallic)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                    mroh.CopyRedToBlue(tmp);
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.roughness)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                    mroh.CopyRedToGreen(tmp);
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.occlusion)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                    mroh.CopyRedToRed(tmp);
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.height)
                        {
                            if (mroh == null)
                            {
                                mroh = new RawBitmap(on.Width, on.Height);
                            }

                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    RawBitmap tmp = new RawBitmap(on.Width, on.Height, bits);
                                    mroh.CopyRedToAlpha(tmp);
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                        else if (on.OutType == OutputType.thickness)
                        {
                            RawBitmap bmp = null;
                            byte[] bits = on.GetPreview(on.Width, on.Height);

                            if (bits != null)
                            {
                                var t = Task.Run(() =>
                                {
                                    bmp = new RawBitmap(on.Width, on.Height, bits);
                                    var src = bmp.ToBitmap();

                                    using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_thickness.png"), FileMode.OpenOrCreate))
                                    {
                                        src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                    }

                                    src.Dispose();
                                });
                                runningTasks.Enqueue(t);
                            }
                        }
                    }
                }
            }

            int extra = (mroh != null) ? 1 : 0;
            int totalTasks = runningTasks.Count + extra;

            ProgressChanged(0, totalTasks, 0);

            return Task.Run(async () =>
            {
                while (runningTasks.Count > 0)
                {
                    i = totalTasks - runningTasks.Count + 1;

                    Task t = runningTasks.Dequeue();

                    ProgressChanged(i, totalTasks, (float)i / (float)totalTasks);

                    if (!t.IsCompleted && !t.IsCanceled)
                    {
                        await t;
                    }
                }

                if (mroh != null)
                {
                    var src = mroh.ToBitmap();

                    using (FileStream fs = new FileStream(System.IO.Path.Combine(path, name + "_occroughmetheight.png"), FileMode.OpenOrCreate))
                    {
                        src.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    src.Dispose();
                }
            });
        }
    }
}
