using System.ComponentModel;

namespace Ecommerce.Domain.Shared;

public enum FileType
{
    /// <summary>
    /// Represents an image file with the extension ".jpg".
    /// </summary>
    [Description(".jpg,.jpeg,.png")]
    Image
}