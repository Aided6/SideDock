using System.Windows.Media.Imaging;
using SideDock.Services;

namespace SideDock.Tests;

[TestClass]
public sealed class ShellIconServiceTests
{
    [TestMethod]
    public void Program_shortcut_uses_target_icon_without_arrow()
    {
        using var directory = new TempDirectory();
        var target = Path.Combine(Environment.SystemDirectory, "notepad.exe");
        var shortcut = Path.Combine(directory.Path, "Editor.lnk");
        new ShortcutWriter().Create(target, shortcut);

        var original = (BitmapSource)ShellIconService.GetIcon(target)!;
        var linked = (BitmapSource)ShellIconService.GetIcon(shortcut)!;
        Assert.AreEqual(original.PixelWidth, linked.PixelWidth);
        Assert.AreEqual(original.PixelHeight, linked.PixelHeight);
        var stride = (original.PixelWidth * original.Format.BitsPerPixel + 7) / 8;
        var expected = new byte[stride * original.PixelHeight];
        var actual = new byte[expected.Length];
        original.CopyPixels(expected, stride, 0);
        linked.CopyPixels(actual, stride, 0);
        CollectionAssert.AreEqual(expected, actual);
    }
}
