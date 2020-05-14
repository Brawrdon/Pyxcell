using Pyxcell;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace PyxcellTests
{
    public class DecoderTests
    {
        private readonly DecodedImage _decodedImage;

        public DecoderTests(ITestOutputHelper output)
        {
            var decoder = new Decoder("EncodedTestImage.png");
            _decodedImage = decoder.Decode();
        }

        [Fact]
        public void DecodeShortStory()
        {
            var decoder = new Decoder("short.png");
            var decodedImage = decoder.Decode();

            // Assumes character encoding is correct if text decodes successfully.
            Assert.Equal("John McGahern and Annie Proulx are among my favourite authors, but to dispel gloom I " +
                         "choose this story from Jane Gardam?s 1980 collection The Sidmouth Letters. Reading this gleeful " +
                         "story in my expatriate days, I recognised the cast of ?diplomatic wives?, trailing inebriate husbands " +
                         "through the ruins of empire. Mostly dialogue, it is a deft, witty tale in which a small kindness ? though " +
                         "not by a diplomatic wife ? pays off 40 years later. I must have read it a dozen times, to see how its note " +
                         "is sustained and the surprise is sprung; every time it makes me smile with delight. Hilary Mantel. " +
                         "This great and underrated masterpiece is a meditation on good and evil and especially about the way " +
                         "that people?s expectations and assumptions about us may wear us down and eventually force us into " +
                         "compliance with their view. But it is a much deeper and more biblical story than that and, like any " +
                         "great work of art, resists reduction. Berriault, who died in 1999, is known as a San Francisco writer. " +
                         "A wonderful sampling of her stories is available in Women in Their Beds: New & Selected Stories. George Saunders." +
                         " Among the handful of short stories closest to my heart, I?ve chosen ?The Love of a Good Woman? by Canadian writer " +
                         "Munro, from her 1998 collection of that name. It?s about a murder ? probably it?s a murder, because nothing is certain ? " +
                         "and a love match that depends on keeping that murder secret. Like so many of Munro?s stories, this one has the scope of a novel " +
                         "yet never feels hurried or crowded. The sociology of a small town in rural Ontario is caught on the wing in the loose weave of her narration;" +
                         " the story takes in whole lifetimes, and yet its pace is also exquisitely slow, carrying us deep inside particular moments. A " +
                         "woman moves among the willows beside a river at night, making up her mind. Tessa Hadley. Born in Palermo in 1896.",
                decodedImage.Text);
        }

        [Fact]
        public void DecodeText()
        {
            // Assumes character encoding is correct if text decodes successfully.
            Assert.Equal("You need to install the plugin and activate it for your stream on their website." +
                         "Sometimes, I really like it when my friends buy me food without me asking." +
                         "Don't you think it's great that we can all have a blast on this tiny rock." +
                         "I like cheese most days. But sometimes, I don't.", _decodedImage.Text);
        }


        [Fact]
        public void DecodeColourPalettes()
        {
            Assert.Equal(10, _decodedImage.ColourPalette.Colours.Count);
            Assert.Equal(3, _decodedImage.KeywordColourPalette.Colours.Count);
        }

        [Fact]
        public void DecodeKeywords()
        {
            Assert.Equal(3, _decodedImage.Keywords.Count);
            Assert.Equal("activate", _decodedImage.Keywords[0].Word);
            Assert.Equal(Rgba32.Black, _decodedImage.Keywords[0].Colour);

            Assert.Equal("rock", _decodedImage.Keywords[1].Word);
            Assert.Equal(Rgba32.Silver, _decodedImage.Keywords[1].Colour);

            Assert.Equal("cheese", _decodedImage.Keywords[2].Word);
            Assert.Equal(Rgba32.Yellow, _decodedImage.Keywords[2].Colour);
        }
    }
}