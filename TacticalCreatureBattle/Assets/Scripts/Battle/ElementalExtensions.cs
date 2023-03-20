public static class ElementalExtensions
{
    public static bool HasResistance(this Element thisElement, Element otherElement)
    {
        return thisElement switch
        {
            Element.Aluminum => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => true,
                Element.Paper => false,
                Element.Plastic => true,
                Element.Steel => false,
                _ => false
            },
            Element.Cardboard => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => false,
                Element.Plastic => false,
                Element.Steel => true,
                _ => false
            },
            Element.Glass => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => true,
                Element.Glass => false,
                Element.Paper => true,
                Element.Plastic => true,
                Element.Steel => false,
                _ => false
            },
            Element.Paper => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => true,
                Element.Glass => true,
                Element.Paper => false,
                Element.Plastic => false,
                Element.Steel => false,
                _ => false
            },
            Element.Plastic => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => true,
                Element.Plastic => false,
                Element.Steel => true,
                _ => false
            },
            Element.Steel => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => true,
                Element.Paper => true,
                Element.Plastic => false,
                Element.Steel => false,
                _ => false
            },
            _ => false
        };
    }

    public static bool HasWeakness(this Element thisElement, Element otherElement)
    {
        return thisElement switch
        {
            Element.Aluminum => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => true,
                Element.Plastic => false,
                Element.Steel => true,
                _ => false
            },
            Element.Cardboard => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => true,
                Element.Plastic => true,
                Element.Steel => false,
                _ => false
            },
            Element.Glass => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => false,
                Element.Plastic => false,
                Element.Steel => true,
                _ => false
            },
            Element.Paper => otherElement switch
            {
                Element.Aluminum => true,
                Element.Cardboard => false,
                Element.Glass => false,
                Element.Paper => true,
                Element.Plastic => false,
                Element.Steel => true,
                _ => false
            },
            Element.Plastic => otherElement switch
            {
                Element.Aluminum => true,
                Element.Cardboard => false,
                Element.Glass => true,
                Element.Paper => false,
                Element.Plastic => false,
                Element.Steel => false,
                _ => false
            },
            Element.Steel => otherElement switch
            {
                Element.Aluminum => false,
                Element.Cardboard => true,
                Element.Glass => false,
                Element.Paper => false,
                Element.Plastic => true,
                Element.Steel => false,
                _ => false
            },
            _ => false
        };
    }
}
