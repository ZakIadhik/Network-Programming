using FavoriteMoviesApp.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FavoriteMoviesApp.Services
{
    public class PdfService
    {
        public byte[] GeneratePdf(IEnumerable<MovieDto> movies, string userName)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Favorite Movies for {userName}")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Title").SemiBold();
                                header.Cell().Text("Genre").SemiBold();
                                header.Cell().Text("Year").SemiBold();
                                header.Cell().Text("Director").SemiBold();

                                header.Cell().ColumnSpan(4)
                                    .PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
                            });

                            foreach (var movie in movies)
                            {
                                table.Cell().Text(movie.Title);
                                table.Cell().Text(movie.Genre);
                                table.Cell().Text(movie.Year.ToString());
                                table.Cell().Text(movie.Director);

                                table.Cell().ColumnSpan(4)
                                    .PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}