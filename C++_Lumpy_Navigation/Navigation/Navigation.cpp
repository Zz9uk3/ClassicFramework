#include "Navigation.h"

#include "MoveMap.h"
#include "PathFinder.h"

using namespace std;

Navigation* Navigation::s_singletonInstance = NULL;

Navigation* Navigation::GetInstance()
{
	if (s_singletonInstance == NULL)
		s_singletonInstance = new Navigation();
	return s_singletonInstance;
}

void Navigation::Initialize()
{
	dtAllocSetCustom(dtCustomAlloc, dtCustomFree);
	lastMapId = -1;
}

void Navigation::Release()
{
	MMAP::MMapFactory::createOrGetMMapManager()->~MMapManager();
}

int Navigation::CalculateStraightPath(unsigned int mapId, XYZ start, XYZ end)
{
	return Navigation::CalculatePath(mapId, start, end, true);
}

int Navigation::CalculateSmoothPath(unsigned int mapId, XYZ start, XYZ end)
{
	return Navigation::CalculatePath(mapId, start, end, false);
}

int Navigation::CalculatePath(unsigned int mapId, XYZ start, XYZ end, bool useStraightPath)
{
	MMAP::MMapManager* manager = MMAP::MMapFactory::createOrGetMMapManager();
	if (mapId != lastMapId)
	{
		// ToDo: Check if unload was successful. Problem => first map won't be loaded
		manager->unloadMap(lastMapId);
	}

	int gridX = (int)(32 - start.X / SIZE_OF_GRIDS);
	int gridY = (int)(32 - start.Y / SIZE_OF_GRIDS);
	if (!manager->loadMap(mapId, gridX, gridY))
	{
		MessageBox(0, "X: " + gridX, "X", 0);
		MessageBox(0, "Y: " + gridY, "Y", 0);
		return 0;
	}

	// Load all surrounding map tiles
	manager->loadMap(mapId, gridX + 0, gridY - 1);
	manager->loadMap(mapId, gridX + 1, gridY - 1);
	manager->loadMap(mapId, gridX + 1, gridY + 0);
	manager->loadMap(mapId, gridX + 1, gridY + 1);
	manager->loadMap(mapId, gridX + 0, gridY + 1);
	manager->loadMap(mapId, gridX - 1, gridY + 1);
	manager->loadMap(mapId, gridX - 1, gridY + 0);
	manager->loadMap(mapId, gridX - 1, gridY - 1);

	// ToDo: Remove instanceId
	PathFinder pathFinder(mapId, 1);
	pathFinder.setUseStrightPath(useStraightPath);
	
	if (!pathFinder.calculate(start.X, start.Y, start.Z, end.X, end.Y, end.Z))
	{
		return 0;
	}

	PointsArray pointPath = pathFinder.getPath();

	if (pointPath.size() == 0)
	{
		return 0;
	}
	
	delete[] currentPath;
	currentPath = new XYZ[pointPath.size()];

	for (unsigned int i = 0; i < pointPath.size(); ++i)
	{
		currentPath[i].X = pointPath[i].x;
		currentPath[i].Y = pointPath[i].y;
		currentPath[i].Z = pointPath[i].z;
	}

	return pointPath.size();
}

void Navigation::GetPath(XYZ* path, int length)
{
	for (int i = 0; i < length; i++)
	{
		path[i] = currentPath[i];
	}
}
