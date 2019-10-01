using System;
using System.Collections.Generic;
using src.level.generator.elements;

namespace src.level.generator.levels
{
    public class LevelGenerator
    {
        const int maxTriesGetFreePosition = 20;
        const float factorRadiusGravity = 1.0f;
        public const float factorForceRadiusGoal = 1.0f;

        public int timeInMs = 0;

        protected List<List<Element>> elementsTimeLine;
        protected List<TimeLineEvent> eventsTimeLine;
        protected List<int> eventCandidates;        
        protected int timeStep = 0;
      

        public LevelGenerator()
        {
            elementsTimeLine = new List<List<Element>>();
            eventsTimeLine = new List<TimeLineEvent>();
            eventCandidates = new List<int>();
            Element.idCounter = 0;
        }

        public string CreateLevel(int rndStart, int difficulty)
        {
            DateTime startTime = DateTime.Now;
            Randoms.Init(10*rndStart+difficulty);

            timeStep = 0;
            CreateGoals(difficulty);

            while (FindEventCandidates(difficulty))
            {
                CopyElements();
                timeStep++;
                ChooseEvent();
            }

            // write level xml
            LevelWriter xmlWriter = new LevelWriter();
            string xml = xmlWriter.writeLevel(rndStart, difficulty, elementsTimeLine[timeStep]);

            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime - startTime;
            timeInMs = (int)span.TotalMilliseconds;

            return xml;
        }


        public void CopyElements()
        {
            List<Element> elemList = new List<Element>();
            foreach (Element elem in elementsTimeLine[timeStep])
            {
                elemList.Add(elem);
            }
            elementsTimeLine.Add(elemList);
        }


        private bool FindEventCandidates(int difficulty)
        {
            // find all candidates for events
            eventCandidates = new List<int>();

            foreach (Element elem in elementsTimeLine[timeStep])
            {
                switch (elem.type)
                {
                    case ElementType.Goal:
                        Goal goal = (Goal)elem;
                        if (goal.counterTimed < goal.counter)
                        {
                            eventCandidates.Add(elem.id);
                        }
                    break;

                    case ElementType.ColliderBody:
                        if (Randoms.getProbality(2 * LevelHelper.maxDifficulty - difficulty))
                        {
                            eventCandidates.Add(elem.id);
                        }
                    break;

                }
            }

            return (eventCandidates.Count > 0);
        }

        protected void ChooseEvent()
        {
            int cnt = eventCandidates.Count;
            int rdx = Randoms.getInt(0, cnt-1);
            int id = eventCandidates[rdx];

            Element elem = getElement(timeStep, id);

            switch (elem.type)
            {
                case ElementType.Goal:
                    Goal goal = (Goal)elem;
                    goal.counterTimed++;
                    Position newPos = getFreePosition(ColliderBody.posRadius, goal.getPosition(), 
                                        RadialGravity.forceRadius);
                    ColliderBody colliderBody = new ColliderBody(newPos, goal.color);
                    elementsTimeLine[timeStep].Add(colliderBody);

                    eventsTimeLine.Add(new TimeLineEvent(colliderBody.id, goal.id));

                    AddGravity(newPos, goal.color);
                    break;

                case ElementType.ColliderBody:
                    ColliderBody coll = (ColliderBody)elem;
                    ElementColor oldColor = coll.color;
                    Position oldPos = new Position(coll.positionX, coll.positionY);
                    ColorChanger chg = new ColorChanger(oldPos, oldColor);
                    elementsTimeLine[timeStep].Add(chg);

                    elementsTimeLine[timeStep].Remove(coll);
                    Position newPosColl = getFreePosition(ColliderBody.posRadius, oldPos, RadialGravity.forceRadius);
                    ElementColor newColor = Randoms.getColor(new List<ElementColor>() { oldColor });
                    ColliderBody newColl = new ColliderBody(newPosColl, newColor,coll.id);                
                    elementsTimeLine[timeStep].Add(newColl);

                    eventsTimeLine.Add(new TimeLineEvent(coll.id, chg.id));

                    AddGravity(newPosColl, newColor);
                    break;
            }
        }


        private void AddGravity(Position pos1, Position pos2, ElementColor color)
        {
            Position center = LevelHelper.getCenter(pos1, pos2);
            float radius = LevelHelper.getDistance(pos1, pos2) * factorRadiusGravity;
            Position posRG = Randoms.getPositionInCircle(center, radius);
            float dist1 = LevelHelper.getDistance(pos1, posRG);
            float dist2 = LevelHelper.getDistance(pos2, posRG);
            float dist = Math.Max(dist1, dist2);
            float strength = GravityForce.getForce(dist);
            float strengthRadius = GravityForce.getForceRadius(strength);

            RadialGravity rg = new RadialGravity(posRG, strength, strengthRadius, new List<ElementColor>() {color });
            elementsTimeLine[timeStep].Add(rg);
        }

        private void AddGravity(Position pos, ElementColor color)
        {
            if (findRadialGravity(pos, RadialGravity.forceRadius, color)) return;

            Position posRG = Randoms.getPositionInCircle(pos, RadialGravity.forceRadius);
            float strength = GravityForce.force[Randoms.getInt(0,2)];

            RadialGravity rg = new RadialGravity(posRG, strength, RadialGravity.forceRadius, new List<ElementColor>() { color });
            elementsTimeLine[timeStep].Add(rg);
        }
        private bool findRadialGravity(Position pos, float radius, ElementColor color)
        {
            foreach (Element elem in elementsTimeLine[timeStep])
            {
                if (elem.type == ElementType.RadialGravity)
                {
                    float dist = LevelHelper.getDistance(pos, elem.getPosition());
                    if (dist < radius)
                    {
                        RadialGravity rg = (RadialGravity) elem;
                        if (rg.containsColor(color))
                        {
                            return true;
                        }
                        else
                        {
                            // add color to existing RadialGravity
                            elementsTimeLine[timeStep].Remove(rg);
                            List<ElementColor> colors = new List<ElementColor>();
                            colors.Add(rg.colors[0]);
                            colors.Add(color);
                            RadialGravity rgNew = new RadialGravity(rg.getPosition(), rg.strength, rg.strengthRadius, colors, rg.id);
                            elementsTimeLine[timeStep].Add(rgNew);
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        private void CreateGoals(int difficulty)
        {
            // Goals
            elementsTimeLine.Add(new List<Element>());
            int cntGoals = 1 + Math.Min((int)ElementColor._last_ - 2, (difficulty / 3));
            List<ElementColor> colors = new List<ElementColor>();
            for (int i = 1; i <= cntGoals; i++)
            {
                int counter = Randoms.getInt(1, difficulty);
                ElementColor color = Randoms.getColor(colors);
                colors.Add(color);
                Position pos = getFreePosition(Goal.posRadius);
                Goal goal = new Goal(pos, counter, color);
                elementsTimeLine[timeStep].Add(goal);
            }

            // Dummy Event
            eventsTimeLine.Add(new TimeLineEvent(0, 0));
        }


        protected Position getFreePosition(float radiusElem, Position centerCircle = null, float radiusCircle = 0)
        {
            bool bContinue = true;
            int cnt = 0;
            Position pos = null;
            while (bContinue)
            {
                if (centerCircle == null)
                {
                    pos = Randoms.getPosition(radiusElem);
                }
                else
                {
                    // ToDo: Position maybe out of screen
                    pos = Randoms.getPositionInCircle(centerCircle,radiusCircle);
                }

                cnt++;
                bContinue = false;
                foreach (Element elem in elementsTimeLine[timeStep])
                {
                    Position elemPos = elem.getPosition();
                    float distance = LevelHelper.getDistance(pos, elemPos);
                    if (distance <= elem.radius + radiusElem)
                    {
                        bContinue = true;
                    }
                }

                if (cnt >= maxTriesGetFreePosition)
                {
                    throw new Exception("getFreePosition: Max tries reached");
                }
            }
            return pos;
        }

        public List<Element> getLevelElements()
        {
            return elementsTimeLine[timeStep];
        }

        public TimeLineEvent getTimeLineEvent(int timeStep)
        {
            return eventsTimeLine[timeStep];
        }

        public List<Element> getLevelElements(int time)
        {
            return elementsTimeLine[time];
        }

        public int getTimeSteps()
        {
            return elementsTimeLine.Count - 1;
        }

        public Element getElement(int timeStep, int id)
        {
            foreach (Element elem in elementsTimeLine[timeStep])
            {
                if (elem.id == id) return elem;
            }

            return null;
        }
    }
}
