using System.Collections.Generic;

namespace SteganographyInPicture.Models;

public record StudyMenuItemModel(
    string MenuItemName,
    List<Paragraph> Paragraphs);