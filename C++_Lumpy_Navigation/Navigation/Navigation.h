#ifndef NAVIGATION_H
#define NAVIGATION_H

typedef struct _XYZ { float X; float Y; float Z; } XYZ;

class Navigation
{
public:
	static const int ERROR = -1;
	static const int ERROR_LOAD_MAP = -2;
	static const int ERROR_PATH_CALCULATION = -3;

public:
	static Navigation* GetInstance();
	void Initialize();
	void Release();
	int CalculateStraightPath(unsigned int mapId, XYZ start, XYZ end);
	int CalculateSmoothPath(unsigned int mapId, XYZ start, XYZ end);
	void GetPath(XYZ* path, int length);

private:
	static Navigation* s_singletonInstance;
	unsigned int lastMapId;
	XYZ* currentPath;

	int CalculatePath(unsigned int mapId, XYZ start, XYZ end, bool useStraightPath);
};

#endif