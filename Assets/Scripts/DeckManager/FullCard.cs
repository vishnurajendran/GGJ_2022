using System;

namespace Flippards
{
    [Serializable]
    public class FullCard
    {
        public CardAttributes frontCard;
        public CardAttributes backCard;
        public bool isCardFlipped;

        public FullCard(CardAttributes frontCard, CardAttributes backCard)
        {
            this.frontCard = frontCard;
            this.backCard = backCard;
            this.isCardFlipped = false;
        }

        public FullCard()
        {
            //Debug constructor
        }

        public CardAttributes GetTopCardAttributes()
        {
            return isCardFlipped ? backCard : frontCard;
        }
    }
}