--��������¼� ������
x200001_g_scriptId = 200001


function x200001_OnCounter(uuid, eventId, dungeonId, stageId, starCnt, damage, evaluation, killMonster, value, count)
	local achKey = 0

	local stageType = LuaGetStageType(stageId)
	if stageType == 0 then
		--��ͨ�ؿ�
		--9	achNormalDungeonWin	0	��ͨ�����ۼ�ͨ�ش���
		achKey = 9
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	elseif stageType == 1 then
		--��Ӣ�ؿ�
		--10	achjingyingDungeonWin	0	��Ӣ�����ۼ�ͨ�ش���
		achKey = 10
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	end
	
	--��ɱ����
	--47	achKillMonster	0	��ɱ�������
	achKey = 47
	LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + killMonster)
	
	if damage == 0 then
		--����ͨ�ؼ���
		--48	achUnhurtStageWin	0	����ͨ�ؼ���
		achKey = 48
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	end
	
	if evaluation == 5 then
		--���SSS���� ����
		--49	achStageWinWithSSS	0	SSS����ͨ�ؼ���

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

