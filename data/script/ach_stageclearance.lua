--副本完成事件 计数器
x200001_g_scriptId = 200001


function x200001_OnCounter(uuid, eventId, dungeonId, stageId, starCnt, damage, evaluation, killMonster, value, count)
	local achKey = 0

	local stageType = LuaGetStageType(stageId)
	if stageType == 0 then
		--普通关卡
		--9	achNormalDungeonWin	0	普通副本累计通关次数
		achKey = 9
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	elseif stageType == 1 then
		--精英关卡
		--10	achjingyingDungeonWin	0	精英副本累计通关次数
		achKey = 10
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	end
	
	--击杀怪物
	--47	achKillMonster	0	击杀怪物计数
	achKey = 47
	LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + killMonster)
	
	if damage == 0 then
		--无伤通关计数
		--48	achUnhurtStageWin	0	无伤通关计数
		achKey = 48
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	end
	
	if evaluation == 5 then
		--获得SSS评价 计数
		--49	achStageWinWithSSS	0	SSS评价通关计数

		achKey = 49
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	end
	
	achKey = 53
	LuaSetAchievementValue(uuid, achKey, LuaGetDungeonAllStar(uuid))
	
	local strMsg = string.format("x200001_OnCounter uuid=%d, eventId=%d, dungeonId=%d, stageId=%d, starCnt=%d, damage=%d, evaluation=%d, killMonster=%d", 
					uuid, eventId, dungeonId, stageId, starCnt, damage, evaluation, killMonster)
	LuaWriteLog(strMsg)
  	return
end

